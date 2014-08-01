using System.Collections;
using Glimpse.AspNet;
using Glimpse.Core.Configuration;

namespace CacheByAttribute.Test.MvcApplication
{
    public class SampleCode
    {
    }

//    public class UserService
//    {
//        /* SNIP*/
//        [Cache]
//        public virtual User GetAnonymoususer()
//        {
//            return this._UserRepository.Get(CustomerConfiguration.AnonymousUserId);
//        }

//        [Cache(IdleExpiryMinutes /ExpiryMinutes/ExpiSeconds)]
//        public virtual User GetAnonymoususer()
//        {
//            return this._UserRepository.Get(CustomerConfiguration.AnonymousUserId);
//        }


//        [Cache(ExpiryHours = 1, ExpiryMinutes = 30, IdleExpiryMinutes = 30)]
//        public virtual IList<HousesForSale> GetHousesForSale(string searchCriteria)
//        {
//             return this._HouseRepository.Get(Status.Current, searchCriteria);
//        }

//        [Cache(AbsoluteKey = "SystemConfiguration")]
//        public SystemConfiguration GetSystemConfiguration()
//        {
//            return this._SystemConfigurationRepository.Get(Constants.SystemConfigurationId);
//        }

//        [CacheRemove(AbsoluteKey = "SystemConfiguration")]
//        public void SaveSystemConfiguration(SystemConfiguration systemConfiguration)
//        {
//            this._SystemConfigurationRepository.Save(systemConfiguration);
//        }


//    }

//    namespace Company.Project.Internationalisation
//    {
//        public class LanguageService
//        {
//            [Cache (KeyPrefix="Caption")]
//            public string GetCaptionValue(string name)
//            {
//                return this._CaptionRepository.GetByName(name).Value;
//            }

//            [CacheRemove(KeyPrefix="Caption"))]
//            public void SaveCaption(Caption caption)
//            {
//                this._CaptionRepository.Save(caption);
//            }
//        }
//    }



//    namespace Company.Project
//    {
//        public class CustomerConfigurationService
//        {
//            [Cache (KeyPrefix="CustomerConfiguration")]
//            public CustomerConfiguration GetCustomerConfiguration(int id)
//            {
//                return this._CustomerConfigurationRepository.Get(id);
//            }

//            [CacheRemove(KeyPrefix="CustomerConfiguration"))]
//            public void SaveCustomerConfiguration(CustomerConfiguration customerConfiguration)
//            {
//                this._CustomerConfigurationRepository.Save(customerConfiguration);
//            }
//        }
//    }


//}

}