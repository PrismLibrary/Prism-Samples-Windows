// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace AdventureWorks.UILogic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Windows.ApplicationModel.Resources;

    /// <summary>
    /// The BindableValidator class run validation rules of an entity, and stores a collection of errors of the properties that did not pass validation.
    /// The validation is run on each property change or whenever the ValidateProperties method is called.
    /// It also provides an indexer property, that uses the property names as keys and return the error list for the specified property.
    /// </summary>
    public class BindableValidator : INotifyPropertyChanged
    {
        /// <summary>
        /// Represents a collection of empty error values.
        /// </summary>
        public static readonly Collection<string> EmptyErrorsCollection = new Collection<string>(new List<string>());

        private readonly INotifyPropertyChanged _entityToValidate;

        private IDictionary<string, Collection<string>> _errors = new Dictionary<string, Collection<string>>();

        private Func<string, string, string> _getResourceDelegate;

        /// <summary>
        /// Initializes a new instance of the BindableValidator class with the entity to validate.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate</param>
        /// <param name="getResourceDelegate">A delegate that returns a string resource given a resource map Id and resource Id</param>
        /// <exception cref="ArgumentNullException">When <paramref name="entityToValidate"/> is <see langword="null" />.</exception>
        public BindableValidator(INotifyPropertyChanged entityToValidate, Func<string, string, string> getResourceDelegate)
            : this(entityToValidate)
        {
            this._getResourceDelegate = getResourceDelegate;
        }

        /// <summary>
        /// Initializes a new instance of the BindableValidator class with the entity to validate.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate</param>
        /// <exception cref="ArgumentNullException">When <paramref name="entityToValidate"/> is <see langword="null" />.</exception>
        public BindableValidator(INotifyPropertyChanged entityToValidate)
        {
            if (entityToValidate == null)
            {
                throw new ArgumentNullException("entityToValidate");
            }

            this._entityToValidate = entityToValidate;
            this.IsValidationEnabled = true;
            this._getResourceDelegate = (mapId, key) =>
            {
                var resourceLoader = ResourceLoader.GetForCurrentView(mapId);
                return resourceLoader.GetString(key);
            };
        }

        /// <summary>
        /// Multicast event for errors change notifications.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the list of errors per property.
        /// </summary>
        /// <value>
        /// The dictionary of property names and errors collection pairs.
        /// </value>
        public IDictionary<string, Collection<string>> Errors
        {
            get { return this._errors; }
        }

        /// <summary>
        /// Returns true if the Validation functionality is enabled. Otherwise, false.
        /// </summary>
        public bool IsValidationEnabled { get; set; }

        /// <summary>
        /// Returns the errors of the property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The errors of the property, if it has errors. Otherwise, the BindableValidator.EmptyErrorsCollection.</returns>
        public Collection<string> this[string propertyName]
        {
            get
            {
                return this._errors.ContainsKey(propertyName) ? this._errors[propertyName] : EmptyErrorsCollection;
            }
        }

        /// <summary>
        /// Returns a new ReadOnlyDictionary containing all the errors of the Entity, separated by property.
        /// </summary>
        /// <returns>
        /// A ReadOnlyDictionary that contains a KeyValuePair for each property with errors. 
        /// Each KeyValuePair has a property name as the key, and the value is the collection of errors of that property.
        /// </returns>
        public Dictionary<string, Collection<string>> GetAllErrors()
        {
            return new Dictionary<string, Collection<string>>(this._errors);
        }

        /// <summary>
        /// Updates the errors collection of the entity, notifying if the errors collection has changed.
        /// </summary>
        /// <param name="entityErrors">The collection of errors for the entity.</param>
        public void SetAllErrors(IDictionary<string, Collection<string>> entityErrors)
        {
            if (entityErrors == null)
            {
                throw new ArgumentNullException("entityErrors");
            }

            this._errors.Clear();

            foreach (var item in entityErrors)
            {
                this.SetPropertyErrors(item.Key, item.Value);
            }

            this.OnPropertyChanged("Item[]");
            this.OnErrorsChanged(string.Empty);
        }

        /// <summary>
        /// Validates the property, based on the rules set in the property ValidationAttributes attributes. 
        /// It updates the errors collection with the new validation results (notifying if necessary). 
        /// </summary>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <returns>True if the property is valid. Otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="propertyName"/> is <see langword="null" /> or an empty string value.</exception>
        /// <exception cref="ArgumentException">When the <paramref name="propertyName"/> parameter does not match any property name.</exception>
        public bool ValidateProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            var propertyInfo = this._entityToValidate.GetType().GetRuntimeProperty(propertyName);
            if (propertyInfo == null)
            {
                var errorString = this._getResourceDelegate(PrismConstants.StoreAppsInfrastructureResourceMapId, "InvalidPropertyNameException");

                throw new ArgumentException(errorString, propertyName);
            }

            var propertyErrors = new List<string>();
            bool isValid = this.TryValidateProperty(propertyInfo, propertyErrors);
            bool errorsChanged = this.SetPropertyErrors(propertyInfo.Name, propertyErrors);

            if (errorsChanged)
            {
                this.OnErrorsChanged(propertyName);
                this.OnPropertyChanged(string.Format(CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
            }

            return isValid;
        }

        /// <summary>
        /// Validates all the properties decorated with the ValidationAttribute attribute.
        /// It updates each property errors collection with the new validation results (notifying if necessary). 
        /// </summary>
        /// <returns>True if the property is valid. Otherwise, false.</returns>
        public bool ValidateProperties()
        {
            var propertiesWithChangedErrors = new List<string>();

            // Get all the properties decorated with the ValidationAttribute attribute.
            var propertiesToValidate = this._entityToValidate.GetType().GetRuntimeProperties(); 

            // TODO: Validate the below once data annotations are available
            //// .Where(c => c.GetCustomAttributes(typeof(ValidationAttribute)).Any());

            foreach (PropertyInfo propertyInfo in propertiesToValidate)
            {
                var propertyErrors = new List<string>();
                this.TryValidateProperty(propertyInfo, propertyErrors);

                // If the errors have changed, save the property name to notify the update at the end of this method.
                bool errorsChanged = this.SetPropertyErrors(propertyInfo.Name, propertyErrors);
                if (errorsChanged && !propertiesWithChangedErrors.Contains(propertyInfo.Name))
                {
                    propertiesWithChangedErrors.Add(propertyInfo.Name);
                }
            }

            // Notify each property whose set of errors has changed since the last validation.  
            foreach (string propertyName in propertiesWithChangedErrors)
            {
                this.OnErrorsChanged(propertyName);
                this.OnPropertyChanged(string.Format(CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
            }

            return this._errors.Values.Count == 0;
        }

        /// <summary>
        /// Performs a validation of a property, adding the results in the propertyErrors list. 
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo of the property to validate</param>
        /// <param name="propertyErrors">A list containing the current error messages of the property.</param>
        /// <returns>True if the property is valid. Otherwise, false.</returns>
        private bool TryValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors)
        {
            // TODO: Validate the below once data annotations are available

            ////var results = new List<ValidationResult>();
            ////var context = new ValidationContext(_entityToValidate) { MemberName = propertyInfo.Name };
            ////var propertyValue = propertyInfo.GetValue(_entityToValidate);

            ////// Validate the property
            ////bool isValid = Validator.TryValidateProperty(propertyValue, context, results);

            ////if (results.Any())
            ////{
            ////    propertyErrors.AddRange(results.Select(c => c.ErrorMessage));
            ////}

            ////return isValid;
            return true;
        }

        /// <summary>
        /// Updates the errors collection of the property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyNewErrors">The new collection of property errors.</param>
        /// <returns>True if the property errors have changed. Otherwise, false.</returns>
        private bool SetPropertyErrors(string propertyName, IList<string> propertyNewErrors)
        {
            bool errorsChanged = false;

            // If the property does not have errors, simply add them
            if (!this._errors.ContainsKey(propertyName))
            {
                if (propertyNewErrors.Count > 0)
                {
                    this._errors.Add(propertyName, new Collection<string>(propertyNewErrors));
                    errorsChanged = true;
                }
            }
            else
            {
                // If the property has errors, check if the number of errors are different.
                // If the number of errors is the same, check if there are new ones
                if (propertyNewErrors.Count != this._errors[propertyName].Count || this._errors[propertyName].Intersect(propertyNewErrors).Count() != propertyNewErrors.Count())
                {
                    if (propertyNewErrors.Count > 0)
                    {
                        this._errors[propertyName] = new Collection<string>(propertyNewErrors);
                    }
                    else
                    {
                        this._errors.Remove(propertyName);
                    }

                    errorsChanged = true;
                }
            }

            return errorsChanged;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.</param>
        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Notifies listeners that the errors of a property have changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.</param>
        private void OnErrorsChanged(string propertyName)
        {
            var eventHandler = this.ErrorsChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}
