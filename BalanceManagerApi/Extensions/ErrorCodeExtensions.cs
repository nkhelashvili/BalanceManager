using Balances;
using BalanceManagerApi.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceManagerApi.Extensions
{
    public static class ErrorCodeExtensions
    {
        public static string GetErrorDescription(this ErrorCode decreaseBalanceResult)
        {
            string errorDescription = "";

            switch (decreaseBalanceResult)
            {
                case ErrorCode.DuplicateTransactionId:
                    errorDescription = ErrorConstants.DuplicateTransactionIdText;
                    break;
                case ErrorCode.TransactionRejected:
                    errorDescription = ErrorConstants.TransactionRejectedText;
                    break;
                case ErrorCode.NotEnoughtBalance:
                    errorDescription = ErrorConstants.NotEnoughtBalanceText;
                    break;
                case ErrorCode.TransactionNotFound:
                    errorDescription = ErrorConstants.TransactionNotFoundText;
                    break;
                case ErrorCode.TransactionAlreadyMarkedAsRollback:
                    errorDescription = ErrorConstants.TransactionAlreadyMarkedAsRollbackText;
                    break;
                case ErrorCode.TransactionRollbacked:
                    errorDescription = ErrorConstants.TransactionRollbacked;
                    break;
                case ErrorCode.UnknownError:
                default:
                    errorDescription = ErrorConstants.UnknownErrorText;
                    break;
            }

            return errorDescription;
        }
    }
}
