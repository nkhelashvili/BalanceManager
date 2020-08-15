# სატესტო დავალება


# ამოცანა

1. მოახდინეთ RestApi-ის იმპლემენტაცია რომელიც გამოდგავს შემდეგ მეთოდებს:
   * api/balance - აბრუნებს კაზინოს ბალანსს
   * api/withdraw/{{transactionid}}/{{amount}} - აკეთებს თანხის გადარიცხვას კაზინოს ბალანსიდან თამაშის ბალანსზე
   * api/deposit/{{transactionid}}/{{amount}} - აკეთებს თანხის გადმორიცხვას თამაშის ბალანსიდან კაზინოს ბალანსზე
2. აპლიკაცია უნდა იყოს დაწერილი dotnet3.1 ზე
3. RestApi-ს უნდა გააჩნდეს swagger დოკუმენტაცია.

## ბიბლიოეთეკის აღწერა
`BalanceManager.dll` - ში აღწერილია 2 სერვისი (CasinoBalanceManager და GameBalanceManager) მათი ფუნქციონალი არის სრულიად იდენტური.

ორივე სერვისს გააჩნია შემდეგი მეთოდები:

    ErrorCode IncreaseBalance(decimal amount, string transactionId);
    ErrorCode DecreaseBalance(decimal amount, string transactionId);
    ErrorCode Rollback(string transactionId);
    ErrorCode CheckTransaction(string transactionId);
    decimal GetBalance();

`ErrorCode` არის Enum რომელიც ასე გამოიყურება
    
    Success = 10,
    TransactionRejected = 11,
    NotEnoughtBalance = 12,
    DuplicateTransactionId = 13,
    TransactionNotFound = 14,
    TransactionAlreadyMarkedAsRollback = 15,
    TransactionRollbacked = 16,
    UnknownError = 111

## IncreaseBalance

პარამეტრები:

	amount - ტრანზაქციის თანხა 
	transactionId - ტრანზაქციის უნიკალური id
​
შესაძლო პასუხები:

	Success - ტრანზაქცია წარმატებულია. ბალანსი დარედაქტირდა.
	TransactionRejected - ტრანზაქცია არ გატარდა. ბალანსი არ შეიცვალა.
	DuplicateTransactionId - ტრანზაქციის id უკვე არსებობს ბაზაში. ბალანსი არ შეიცვალა.
	UnknownError - გაურკვეველი შეცდომა, უცნობია ბალანსი შეიცვალა თუ არა.
​
## DecreaseBalance

პარამეტრები:

	amount - ტრანზაქციის თანხა 
	transactionId - ტრანზაქციის უნიკალური id
​
შესაძლო პასუხები:

	Success - ტრანზაქცია წარმატებულია. ბალანსი დარედაქტირდა.
	TransactionRejected - ტრანზაქცია არ გატარდა. ბალანსი არ შეიცვალა.
	DuplicateTransactionId - ტრანზაქციის id უკვე არსებობს ბაზაში. ბალანსი არ შეიცვალა.
	NotEnoughtBalance - ანგარიშზე თანხა არ არის საკმარისი. ბალანსი არ შეიცვალა.
	UnknownError - გაურკვეველი შეცდომა, უცნობია ბალანსი შეიცვალა თუ არა.
​
## Rollback

პარამეტრები:

	transactionId - უნიკალური ტრანზაქციის id
​
შესაძლო პასუხები:

	Success - ტრანზაქცია წარმატებულია. ბალანსი დარედაქტირდა.
	NotEnoughtBalance - ანგარიშზე თანხა არ არის საკმარისი. ბალანსი არ შეიცვალა.
	TransactionNotFound - ტრანზაქცია არ მოიძებნა. ბალანსი არ შეიცვალა.
	TransactionAlreadyMarkedAsRollback - ეს ტრანზაქცია უკვე დაროლბექებულია. ბალანსი არ შეიცვალა.
	UnknownError - გაურკვეველი შეცდომა, უცნობია ბალანსი შეიცვალა თუ არა.
​
## CheckTransaction

პარამეტრები:

	transactionId - უნიკალური ტრანზაქციის id
​
შესაძლო პასუხები:

	Success - ტრანზაქცია წარმატებულია
	TransactionNotFound - ტრანზაქცია ვერ მოიძებნა
	TransactionRollbacked - ტრანზაქცია დაროლბექებულია
​
​
## GetBalance
​
აბრუნებს პირდაპირ თანხას

