using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gali.AppServer.Resources
{
    public class Validator
    {
        public List<ValidatorResult> Items = new List<ValidatorResult>();

        public void Add(ValidatorResult validatorResult)
        {
            this.Items.Add(validatorResult);
        }

        public void Clear(ValidatorResult validatorResult)
        {
            this.Items.Clear();
        }

        public ValidatorResults Validate()
        {
            ValidatorResults validatorResults = new ValidatorResults();
            foreach (var item in Items)
            {
                if (!item.IsValid)
                {
                    validatorResults.IsValid = false;
                    validatorResults.InvalidItems.Add(item);
                }
            }
            return validatorResults;
        }

        public bool ValidateItem(string item)
        {
            bool isValid = true;
            var ItemsFiltered = this.Items.Where(i => i.ItemName == item);
            foreach (var _item in ItemsFiltered)
            {
                if (!_item.IsValid)
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }
    }

    public class ValidatorResult
    {
        public bool IsValid;
        public string Message;
        public string ItemName;
    }

    public class ValidatorResults
    {
        public bool IsValid;
        public List<ValidatorResult> InvalidItems;

        public ValidatorResults()
        {
            this.InvalidItems = new List<ValidatorResult>();
            this.IsValid = true;
        }
    }

    public enum RequiredOnlyTextType { OnlyLetters, OnlyLettersAndNumbers, OnlyLettersNumbersAndUnderscore, OnlyNumbers }
}
