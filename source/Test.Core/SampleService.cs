using System;
using System.Collections.Generic;
using System.Linq;
using CacheByAttribute;

namespace MyProject.MyNamespace
{
    public class SampleService
    {
        private const int GuestUserId = 1;
        private const int SystemSettingsId = 1;
        private readonly CaptionRepository _CaptionRepository;
        private readonly HouseRepository _HouseRepository;
        private readonly SystemSettingsRepository _SystemSettingsRepository;
        private readonly UserRepository _UserRepository;

        #region Cache Domain Object Forever Example

        [Cache]
        public User GetGuestUser()
        {
            return _UserRepository.Get(GuestUserId);
            /* 
             * This method returns the same output every time and we don't need to expire it from cache because the guest user never changes. Therefore, we will cache it indefinitely. 
             * Its cache key is MyProject.MyNamespace.SampleService.GetGuestUser()
             */
        }

        #endregion Cache Domain Object Forever Example

        #region Cache Updatable Domain Example

        public SystemSettings GetSystemSettings(int id)
        {
            return _SystemSettingsRepository.Get(id);
            /*
             * This is a non-cached method that you would use when displaying the Edit System Settings screen.  Being non-cached, it is not a shared object and therefore properties can be 
             * set without worrying about other uses seeing half valid system settings objects,
             */
        }


        [Cache(AbsoluteKey = "SystemSettings")]
        public SystemSettings GetSystemSettings()
        {
            return GetSystemSettings(SystemSettingsId);
            /*
             * This is the method the app calls when it needs to use system settings.  EG. SampleService.GetSystemGettings().ImageUploadPath.
             * The cache key is explicit, therefore it is simply "SystemSettings"
             */
        }


        [CacheRemove(AbsoluteKey = "SystemSettings")]
        public void SaveSystemSettings(SystemSettings systemSettings)
        {
            /*
             * We are removing the system settings from the cache, using it's explicit cache key.
             */
            _SystemSettingsRepository.Save(systemSettings);
        }

        #endregion Cache Updatable Domain Example

        #region Cache with Time Expiry

        [Cache(ExpiryHours = 1, ExpiryMinutes = 30)]
        public IList<House> GetHousesForSale(string searchString)
        {
            /*
             * Removed the cached results after 1.5 hours from the time the item was placed in cache.  
             * The cache key would be "MyProject.MyNamespace.SampleService.GetHousesForSale(searchString)", where searchString is the value passed to the argument.
             */
            return _HouseRepository.GetList(searchString, StatusId.Current);
        }


        [Cache(IdleExpiryMinutes = 30)]
        public IList<House> GetSoldHouses(string searchString)
        {
            /*
             * Removed the cached results if not accessed for 30 minutes. Ideal for objects that don't change  
             * The cache key would be "MyProject.MyNamespace.SampleService.GetSoldHouses(searchString)", where searchString is the value passed to the argument.
             */
            return _HouseRepository.GetList(searchString, StatusId.Sold);
        }

        #endregion Cache with Time Expiry

        #region Cache with expiry on object save

        [Cache(KeyPrefix = "Objects.House")]
        public House GetHouse(int id)
        {
            /*
             * The cache key would be "Objects.House(id)" where id is the value passed in the id attribute. This item will expire if not accessed within 30 minutes
             */
            return _HouseRepository.Get(id);
        }


        [Cache(KeyPrefix = "Objects.House")]
        public void Save(House house)
        {
            /*
             * Because House implements iHasCacheKey, this will remove an item from cache with a key of "Objects.House(house.CacheKey)" where house.CacheKey is the value of house.id
             */
            _HouseRepository.Save(house);
        }

        #endregion Cache with expiry on object save

        #region Cache with regions

        [Cache(Region = "Captions")]
        public string GetCaptionValue(string captionName, Language language, params object[] replaceTags)
        {
            Caption caption = _CaptionRepository.GetByName(captionName);
            LanguageCaption languageCaption = caption.LanguageCaptions.FirstOrDefault(lc => lc.Language == language);
            return languageCaption != null
                       ? string.Format(languageCaption.Value, replaceTags)
                       : string.Format(caption.DefaultValue, replaceTags);

            /*
             * The generated key for this method would be "MyProject.MyNamespace.SampleService.GetCaptionValue(captionName, language.CacheKey, arg1, arg2, arg..n)" 
             * These items are stored in their own region called "Captions" to allow them to be removed easily - by deleting everything from that region.
             */
        }


        [CacheRemoveAllFromRegion("Captions")]
        public void SaveCaption(Caption caption)
        {
            _CaptionRepository.Save(caption);
            /*
             * Remove all items in the cache within the region named "Captions" 
             * 
             */
        }

        #endregion Cache with regions
    }

    #region Dependent Objects

    public class BaseDomainObject : IHasCacheKey
    {
        public int Id { get; set; }
        public string CacheKey
        {
            get { return Id.ToString(); }
        }
    }

    public class User : BaseDomainObject
    {
        public string Name { get; set; }
    }

    public class SystemSettings : BaseDomainObject
    {
        public string Name { get; set; }
    }

    public class House : BaseDomainObject
    {
        public string Address { get; set; }
    }

    public class SystemSettingsRepository
    {
        public SystemSettings Get(int id)
        {
            return new SystemSettings();
        }


        public void Save(SystemSettings systemSettings)
        {
        }
    }

    public enum StatusId
    {
        Current = 1,
        Sold = 2
    }

    public class UserRepository
    {
        public User Get(int id)
        {
            return new User {Id = id, Name = "A User"};
        }
    }

    public class CaptionRepository
    {
        public Caption Get(int id)
        {
            return new Caption {Id = id, Name = "Bob", DefaultValue = "A Caption"};
        }


        public Caption GetByName(string name)
        {
            return new Caption {Name = name, DefaultValue = "A Caption"};
        }


        public void Save(Caption caption)
        {
            throw new NotImplementedException();
        }
    }

    public class HouseRepository
    {
        public IList<House> GetList(string searchString, StatusId statusId)
        {
            return new List<House>();
        }


        public House Get(int id)
        {
            return new House {Id = id, Address = "A User"};
        }


        public void Save(House house)
        {
            throw new NotImplementedException();
        }
    }

    public class Caption : BaseDomainObject
    {
        public IList<LanguageCaption> LanguageCaptions { get; set; }
        public string DefaultValue { get; set; }
        public string Name { get; set; }
    }

    public class LanguageCaption : BaseDomainObject
    {
        public Language Language { get; set; }
        public string Value { get; set; }
    }

    public class Language : BaseDomainObject
    {
        public string Name { get; set; }
    }

    #endregion
}