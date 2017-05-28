namespace C4rm4x.WebApi.Validation.Core
{
    /// <summary>
    /// The validation context
    /// </summary>
    public class ValidationContext
    {
        /// <summary>
        /// Gets the instance to validate
        /// </summary>
        public object InstanceToValidate { get; private set; }

        /// <summary>
        /// Gets the validator selector that determines whether or not a rule should execute.
        /// </summary>
        public IValidatorSelector ValidatorSelector { get; private set; }

        /// <summary>
        /// Constructor (uses default validator selector)
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        public ValidationContext(object objectToValidate)
            : this(objectToValidate, new DefaultValidatorSelector())
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="validatorSelector">The validator selector</param>
        public ValidationContext(
            object objectToValidate,
            IValidatorSelector validatorSelector)
        {
            InstanceToValidate = objectToValidate;
            ValidatorSelector = validatorSelector;
        }
    }

    /// <summary>
    /// The generic version of the validation context
    /// </summary>
    /// <typeparam name="T">Type of object to validate</typeparam>
    public class ValidationContext<T> : ValidationContext
    {
        /// <summary>
        /// Gets the instance to validate
        /// </summary>
        public new T InstanceToValidate { get; private set; }

        /// <summary>
        /// Constructor (uses default validator selector)
        /// </summary>
        /// <param name="instanceToValidate">Object to validate</param>
        public ValidationContext(T instanceToValidate)
            : this(instanceToValidate, new DefaultValidatorSelector())
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instanceToValidate">Object to validate</param>
        /// <param name="validatorSelector">The validator selector</param>
        public ValidationContext(
            T instanceToValidate,
            IValidatorSelector validatorSelector)
            : base(instanceToValidate, validatorSelector)
        {
            InstanceToValidate = instanceToValidate;
        }
    }
}
