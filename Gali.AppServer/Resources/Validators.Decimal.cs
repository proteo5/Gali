using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gali.AppServer.Resources
{
     public static class Decimals
    {
        public static ValidatorResult IsEqualTo(this decimal val, string itemName, string NotValidMessage, decimal value)
        {
            bool isEqualTo = val == value;

            string message = isEqualTo ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isEqualTo, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsNotEqualTo(this decimal val, string itemName, string NotValidMessage, decimal value)
        {
            bool isNotEqualTo = val != value;

            string message = isNotEqualTo ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isNotEqualTo, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsGreaterThan(this decimal val, string itemName, string NotValidMessage, decimal value)
        {
            bool isGraterThan = val > value;

            string message = isGraterThan ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isGraterThan, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsGreaterOrEqualThan(this decimal val, string itemName, string NotValidMessage, decimal value)
        {
            bool isGraterOrEqualThan = val >= value;

            string message = isGraterOrEqualThan ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isGraterOrEqualThan, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsLessThan(this decimal val, string itemName, string NotValidMessage, decimal value)
        {
            bool isLessThan = val < value;

            string message = isLessThan ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isLessThan, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsLessOrEqualThan(this decimal val, string itemName, string NotValidMessage, decimal value)
        {
            bool isLessOrEqualThan = val <= value;

            string message = isLessOrEqualThan ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isLessOrEqualThan, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsInBetween(this decimal val, string itemName, string NotValidMessage, decimal value1, decimal value2)
        {
            bool isInBetween = val >= value1 && val <= value2;

            string message = isInBetween ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isInBetween, ItemName = itemName, Message = message };
        }

    }
}
