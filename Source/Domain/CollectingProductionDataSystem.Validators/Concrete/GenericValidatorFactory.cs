/// <summary>
/// Summary description for GenericValidatorFactory
/// </summary>
namespace CollectingProductionDataSystem.Validators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Validators.Abstract;

    public class GenericValidatorFactory:IValidatorFactory
    {
        private static Dictionary<Type, object> validators = new Dictionary<Type, object>();

        public IValidator<UnitsManualData> UnitManualDataValidator 
        {
            get 
            {
                return this.GetValidator<UnitsManualData>();
            } 
        }
 
        /// <summary>
        /// Gets the validator.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <returns></returns>
        private IValidator<T> GetValidator<T>() 
            where T: class
        {
            if (!validators.ContainsKey(typeof(T)))
            {
                validators.Add(typeof(T), CreateInstanceOfValidator<T>());
            }

            return (IValidator<T>)validators[typeof(T)];
        }
 
        /// <summary>
        /// Creates the instance of validator.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <returns></returns>
        private IValidator<T> CreateInstanceOfValidator<T>() where T:class
        {
            switch (typeof(T).Name)
            {
                case "UnitsManualData":
                    return (IValidator<T>) new UnitsManualDataValidator();
                default:
                    throw new ArgumentException("Not supported validator!");
            }
        }
 
        
 
       
    }
}
