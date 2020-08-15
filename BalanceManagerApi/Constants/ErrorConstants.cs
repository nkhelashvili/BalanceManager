using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceManagerApi.Constants
{
    public static class ErrorConstants
    {
        public const string TransactionRejectedText = "ტრანზაქცია არ გატარდა.ბალანსი არ შეიცვალა.";
        public const string DuplicateTransactionIdText = "ტრანზაქციის id უკვე არსებობს ბაზაში. ბალანსი არ შეიცვალა.";
        public const string NotEnoughtBalanceText = "ანგარიშზე თანხა არ არის საკმარისი.ბალანსი არ შეიცვალა.";
        public const string TransactionAlreadyMarkedAsRollbackText = "ეს ტრანზაქცია უკვე დაროლბექებულია. ბალანსი არ შეიცვალა.";
        public const string UnknownErrorText = "გაურკვეველი შეცდომა, უცნობია ბალანსი შეიცვალა თუ არა.";
        public const string TransactionRollbacked = "ტრანზაქცია დაროლბექებულია.";
        public const string TransactionNotFoundText = "ტრანზაქცია ვერ მოიძებნა.";
    }
}
