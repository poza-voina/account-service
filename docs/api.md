<!-- Generator: Widdershins v4.0.1 -->

<h1 id="accountservice-api">AccountService.Api v1.0</h1>

> Scroll down for code samples, example requests and responses. Select a language for code samples from the tabs above or the mobile navigation menu.

<h1 id="accountservice-api-account">Account</h1>

## post__accounts

> Code samples

`POST /accounts`

*Создает счет*

> Body parameter

```json
{
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "interestRate": 0.1,
  "closingDate": "2019-08-24T14:15:22Z"
}
```

<h3 id="post__accounts-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[CreateAccountCommand](#schemacreateaccountcommand)|false|none|

> Example responses

> 200 Response

```
{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","ownerId":"4d206909-730f-409a-88f6-dcfaa8fc28cc","type":"Checking","currency":"string","balance":0.1,"interestRate":0.1,"openingDate":"2019-08-24T14:15:22Z","closingDate":"2019-08-24T14:15:22Z"}
```

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "balance": 0.1,
  "interestRate": 0.1,
  "openingDate": "2019-08-24T14:15:22Z",
  "closingDate": "2019-08-24T14:15:22Z"
}
```

<h3 id="post__accounts-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[AccountViewModel](#schemaaccountviewmodel)|

<aside class="success">
This operation does not require authentication
</aside>

## put__accounts

> Code samples

`PUT /accounts`

*Обновляет счет*

> Body parameter

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "closingDate": "2019-08-24T14:15:22Z",
  "interestRate": 0.1
}
```

<h3 id="put__accounts-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[PatchAccountCommand](#schemapatchaccountcommand)|false|none|

> Example responses

> 200 Response

```
{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","ownerId":"4d206909-730f-409a-88f6-dcfaa8fc28cc","type":"Checking","currency":"string","balance":0.1,"interestRate":0.1,"openingDate":"2019-08-24T14:15:22Z","closingDate":"2019-08-24T14:15:22Z"}
```

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "balance": 0.1,
  "interestRate": 0.1,
  "openingDate": "2019-08-24T14:15:22Z",
  "closingDate": "2019-08-24T14:15:22Z"
}
```

<h3 id="put__accounts-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[AccountViewModel](#schemaaccountviewmodel)|

<aside class="success">
This operation does not require authentication
</aside>

## get__accounts

> Code samples

`GET /accounts`

*Получает список счетов*

<h3 id="get__accounts-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|query|query|[GetAccountsQuery](#schemagetaccountsquery)|false|none|

> Example responses

> 200 Response

```
[{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","ownerId":"4d206909-730f-409a-88f6-dcfaa8fc28cc","type":"Checking","currency":"string","balance":0.1,"interestRate":0.1,"openingDate":"2019-08-24T14:15:22Z","closingDate":"2019-08-24T14:15:22Z"}]
```

```json
[
  {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
    "type": "Checking",
    "currency": "string",
    "balance": 0.1,
    "interestRate": 0.1,
    "openingDate": "2019-08-24T14:15:22Z",
    "closingDate": "2019-08-24T14:15:22Z"
  }
]
```

<h3 id="get__accounts-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|Inline|

<h3 id="get__accounts-responseschema">Response Schema</h3>

Status Code **200**

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|[[AccountViewModel](#schemaaccountviewmodel)]|false|none|none|
|» id|string(uuid)|false|none|Индентификатор счета|
|» ownerId|string(uuid)|false|none|Идентификатор клиента|
|» type|[AccountType](#schemaaccounttype)|false|none|none|
|» currency|string¦null|true|none|Валюта|
|» balance|number(double)|false|none|Баланс счета|
|» interestRate|number(double)¦null|false|none|Процентная ставка счета|
|» openingDate|string(date-time)|false|none|Дата открытия счета|
|» closingDate|string(date-time)¦null|false|none|Дата закрытия счета|

#### Enumerated Values

|Property|Value|
|---|---|
|type|Checking|
|type|Deposit|
|type|Credit|

<aside class="success">
This operation does not require authentication
</aside>

## delete__accounts_{id}

> Code samples

`DELETE /accounts/{id}`

*Удаляет счет*

<h3 id="delete__accounts_{id}-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|id|path|string(uuid)|true|Идентификатор счета|

> Example responses

> 200 Response

```
{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","ownerId":"4d206909-730f-409a-88f6-dcfaa8fc28cc","type":"Checking","currency":"string","balance":0.1,"interestRate":0.1,"openingDate":"2019-08-24T14:15:22Z","closingDate":"2019-08-24T14:15:22Z"}
```

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "balance": 0.1,
  "interestRate": 0.1,
  "openingDate": "2019-08-24T14:15:22Z",
  "closingDate": "2019-08-24T14:15:22Z"
}
```

<h3 id="delete__accounts_{id}-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[AccountViewModel](#schemaaccountviewmodel)|

<aside class="success">
This operation does not require authentication
</aside>

## get__accounts_{id}_exists

> Code samples

`GET /accounts/{id}/exists`

*Проверяет наличие счета*

<h3 id="get__accounts_{id}_exists-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|id|path|string(uuid)|true|Идентификатор счета|

> Example responses

> 200 Response

```
{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","ownerId":"4d206909-730f-409a-88f6-dcfaa8fc28cc","type":"Checking","currency":"string","balance":0.1,"interestRate":0.1,"openingDate":"2019-08-24T14:15:22Z","closingDate":"2019-08-24T14:15:22Z"}
```

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "balance": 0.1,
  "interestRate": 0.1,
  "openingDate": "2019-08-24T14:15:22Z",
  "closingDate": "2019-08-24T14:15:22Z"
}
```

<h3 id="get__accounts_{id}_exists-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[AccountViewModel](#schemaaccountviewmodel)|

<aside class="success">
This operation does not require authentication
</aside>

<h1 id="accountservice-api-statement">Statement</h1>

## get__statements_{accountId}

> Code samples

`GET /statements/{accountId}`

*Получает выписку по счету*

<h3 id="get__statements_{accountid}-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|accountId|path|string(uuid)|true|Идентификатор счета|
|ownerId|query|string(uuid)|true|Идентификатор клиента|
|startDateTime|query|string(date-time)|false|Начала диапозона выписки|
|endDateTime|query|string(date-time)|false|Конец диапозона выписки|

> Example responses

> 200 Response

```
[{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","ownerId":"4d206909-730f-409a-88f6-dcfaa8fc28cc","type":"Checking","currency":"string","balance":0.1,"interestRate":0.1,"openingDate":"2019-08-24T14:15:22Z","closingDate":"2019-08-24T14:15:22Z","transactions":[{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","bankAccountId":"5b26b598-a880-4e32-8c41-126aa0206857","counterpartyBankAccountId":"cc9b712c-c0c3-4405-a86e-c2690000b458","amount":0.1,"currency":"string","type":"Credit","description":"string","createdAt":"2019-08-24T14:15:22Z","isApply":true}]}]
```

```json
[
  {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
    "type": "Checking",
    "currency": "string",
    "balance": 0.1,
    "interestRate": 0.1,
    "openingDate": "2019-08-24T14:15:22Z",
    "closingDate": "2019-08-24T14:15:22Z",
    "transactions": [
      {
        "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
        "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
        "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
        "amount": 0.1,
        "currency": "string",
        "type": "Credit",
        "description": "string",
        "createdAt": "2019-08-24T14:15:22Z",
        "isApply": true
      }
    ]
  }
]
```

<h3 id="get__statements_{accountid}-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|Inline|

<h3 id="get__statements_{accountid}-responseschema">Response Schema</h3>

Status Code **200**

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|[[AccountWithTransactionsViewModel](#schemaaccountwithtransactionsviewmodel)]|false|none|none|
|» id|string(uuid)|false|none|Индентификатор счета|
|» ownerId|string(uuid)|false|none|Идентификатор клиента|
|» type|[AccountType](#schemaaccounttype)|false|none|none|
|» currency|string¦null|true|none|Валюта|
|» balance|number(double)|false|none|Баланс счета|
|» interestRate|number(double)¦null|false|none|Процентная ставка счета|
|» openingDate|string(date-time)|false|none|Дата открытия счета|
|» closingDate|string(date-time)¦null|false|none|Дата закрытия счета|
|» transactions|[[TransactionViewModel](#schematransactionviewmodel)]¦null|false|none|Коллекция транзакций|
|»» id|string(uuid)|true|none|Идентификатор транзакции|
|»» bankAccountId|string(uuid)|true|none|Идентификатор счета|
|»» counterpartyBankAccountId|string(uuid)¦null|false|none|Идентификатор счета контрагента|
|»» amount|number(double)|true|none|Количество денег|
|»» currency|string¦null|true|none|Валюта|
|»» type|[TransactionType](#schematransactiontype)|true|none|none|
|»» description|string¦null|true|none|Описание|
|»» createdAt|string(date-time)|true|none|Дата создания транзакции|
|»» isApply|boolean|true|none|Флаг принятия транзакции|

#### Enumerated Values

|Property|Value|
|---|---|
|type|Checking|
|type|Deposit|
|type|Credit|
|type|Credit|
|type|Debit|

<aside class="success">
This operation does not require authentication
</aside>

<h1 id="accountservice-api-transaction">Transaction</h1>

## post__transactions_register

> Code samples

`POST /transactions/register`

*Зарегистрировать транзакцию*

> Body parameter

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string"
}
```

<h3 id="post__transactions_register-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[RegisterTransactionCommand](#schemaregistertransactioncommand)|false|none|

> Example responses

> 200 Response

```
{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","bankAccountId":"5b26b598-a880-4e32-8c41-126aa0206857","counterpartyBankAccountId":"cc9b712c-c0c3-4405-a86e-c2690000b458","amount":0.1,"currency":"string","type":"Credit","description":"string","createdAt":"2019-08-24T14:15:22Z","isApply":true}
```

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string",
  "createdAt": "2019-08-24T14:15:22Z",
  "isApply": true
}
```

<h3 id="post__transactions_register-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[TransactionViewModel](#schematransactionviewmodel)|

<aside class="success">
This operation does not require authentication
</aside>

## post__transactions_transfer

> Code samples

`POST /transactions/transfer`

*Перенос между счетами*

> Body parameter

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "description": "string"
}
```

<h3 id="post__transactions_transfer-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[TransferTransactionCommand](#schematransfertransactioncommand)|false|none|

> Example responses

> 200 Response

```
{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","bankAccountId":"5b26b598-a880-4e32-8c41-126aa0206857","counterpartyBankAccountId":"cc9b712c-c0c3-4405-a86e-c2690000b458","amount":0.1,"currency":"string","type":"Credit","description":"string","createdAt":"2019-08-24T14:15:22Z","isApply":true}
```

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string",
  "createdAt": "2019-08-24T14:15:22Z",
  "isApply": true
}
```

<h3 id="post__transactions_transfer-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[TransactionViewModel](#schematransactionviewmodel)|

<aside class="success">
This operation does not require authentication
</aside>

## post__transactions_execute

> Code samples

`POST /transactions/execute`

*Зарегистрировать и выполнить транзакцию*

> Body parameter

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string"
}
```

<h3 id="post__transactions_execute-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[ExecuteTransactionCommand](#schemaexecutetransactioncommand)|false|none|

> Example responses

> 200 Response

```
{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","bankAccountId":"5b26b598-a880-4e32-8c41-126aa0206857","counterpartyBankAccountId":"cc9b712c-c0c3-4405-a86e-c2690000b458","amount":0.1,"currency":"string","type":"Credit","description":"string","createdAt":"2019-08-24T14:15:22Z","isApply":true}
```

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string",
  "createdAt": "2019-08-24T14:15:22Z",
  "isApply": true
}
```

<h3 id="post__transactions_execute-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[TransactionViewModel](#schematransactionviewmodel)|

<aside class="success">
This operation does not require authentication
</aside>

# Schemas

<h2 id="tocS_AccountType">AccountType</h2>
<!-- backwards compatibility -->
<a id="schemaaccounttype"></a>
<a id="schema_AccountType"></a>
<a id="tocSaccounttype"></a>
<a id="tocsaccounttype"></a>

```json
"Checking"

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|string|false|none|none|

#### Enumerated Values

|Property|Value|
|---|---|
|*anonymous*|Checking|
|*anonymous*|Deposit|
|*anonymous*|Credit|

<h2 id="tocS_AccountViewModel">AccountViewModel</h2>
<!-- backwards compatibility -->
<a id="schemaaccountviewmodel"></a>
<a id="schema_AccountViewModel"></a>
<a id="tocSaccountviewmodel"></a>
<a id="tocsaccountviewmodel"></a>

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "balance": 0.1,
  "interestRate": 0.1,
  "openingDate": "2019-08-24T14:15:22Z",
  "closingDate": "2019-08-24T14:15:22Z"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string(uuid)|false|none|Индентификатор счета|
|ownerId|string(uuid)|false|none|Идентификатор клиента|
|type|[AccountType](#schemaaccounttype)|false|none|none|
|currency|string¦null|true|none|Валюта|
|balance|number(double)|false|none|Баланс счета|
|interestRate|number(double)¦null|false|none|Процентная ставка счета|
|openingDate|string(date-time)|false|none|Дата открытия счета|
|closingDate|string(date-time)¦null|false|none|Дата закрытия счета|

<h2 id="tocS_AccountWithTransactionsViewModel">AccountWithTransactionsViewModel</h2>
<!-- backwards compatibility -->
<a id="schemaaccountwithtransactionsviewmodel"></a>
<a id="schema_AccountWithTransactionsViewModel"></a>
<a id="tocSaccountwithtransactionsviewmodel"></a>
<a id="tocsaccountwithtransactionsviewmodel"></a>

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "balance": 0.1,
  "interestRate": 0.1,
  "openingDate": "2019-08-24T14:15:22Z",
  "closingDate": "2019-08-24T14:15:22Z",
  "transactions": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
      "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
      "amount": 0.1,
      "currency": "string",
      "type": "Credit",
      "description": "string",
      "createdAt": "2019-08-24T14:15:22Z",
      "isApply": true
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string(uuid)|false|none|Индентификатор счета|
|ownerId|string(uuid)|false|none|Идентификатор клиента|
|type|[AccountType](#schemaaccounttype)|false|none|none|
|currency|string¦null|true|none|Валюта|
|balance|number(double)|false|none|Баланс счета|
|interestRate|number(double)¦null|false|none|Процентная ставка счета|
|openingDate|string(date-time)|false|none|Дата открытия счета|
|closingDate|string(date-time)¦null|false|none|Дата закрытия счета|
|transactions|[[TransactionViewModel](#schematransactionviewmodel)]¦null|false|none|Коллекция транзакций|

<h2 id="tocS_CreateAccountCommand">CreateAccountCommand</h2>
<!-- backwards compatibility -->
<a id="schemacreateaccountcommand"></a>
<a id="schema_CreateAccountCommand"></a>
<a id="tocScreateaccountcommand"></a>
<a id="tocscreateaccountcommand"></a>

```json
{
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "interestRate": 0.1,
  "closingDate": "2019-08-24T14:15:22Z"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|ownerId|string(uuid)|true|none|Идентификатор клиента|
|type|[AccountType](#schemaaccounttype)|true|none|none|
|currency|string¦null|true|none|Валюта|
|interestRate|number(double)¦null|false|none|Процентная ставка|
|closingDate|string(date-time)¦null|false|none|Дата закрытия счета|

<h2 id="tocS_ExecuteTransactionCommand">ExecuteTransactionCommand</h2>
<!-- backwards compatibility -->
<a id="schemaexecutetransactioncommand"></a>
<a id="schema_ExecuteTransactionCommand"></a>
<a id="tocSexecutetransactioncommand"></a>
<a id="tocsexecutetransactioncommand"></a>

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|bankAccountId|string(uuid)|true|none|Идентификатор счета|
|amount|number(double)|true|none|Количество денег|
|currency|string¦null|true|none|Валюта|
|type|[TransactionType](#schematransactiontype)|true|none|none|
|description|string¦null|true|none|Описание транзакции|

<h2 id="tocS_GetAccountsQuery">GetAccountsQuery</h2>
<!-- backwards compatibility -->
<a id="schemagetaccountsquery"></a>
<a id="schema_GetAccountsQuery"></a>
<a id="tocSgetaccountsquery"></a>
<a id="tocsgetaccountsquery"></a>

```json
{}

```

### Properties

*None*

<h2 id="tocS_PatchAccountCommand">PatchAccountCommand</h2>
<!-- backwards compatibility -->
<a id="schemapatchaccountcommand"></a>
<a id="schema_PatchAccountCommand"></a>
<a id="tocSpatchaccountcommand"></a>
<a id="tocspatchaccountcommand"></a>

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "closingDate": "2019-08-24T14:15:22Z",
  "interestRate": 0.1
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string(uuid)|true|none|Идентификатор счета|
|closingDate|string(date-time)¦null|false|none|Дата закрытия счета|
|interestRate|number(double)¦null|false|none|Процентная ставка|

<h2 id="tocS_RegisterTransactionCommand">RegisterTransactionCommand</h2>
<!-- backwards compatibility -->
<a id="schemaregistertransactioncommand"></a>
<a id="schema_RegisterTransactionCommand"></a>
<a id="tocSregistertransactioncommand"></a>
<a id="tocsregistertransactioncommand"></a>

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|bankAccountId|string(uuid)|true|none|Идентификатор счета|
|counterpartyBankAccountId|string(uuid)¦null|false|none|Идентификатор счета контрагента|
|amount|number(double)|true|none|Количество денег|
|currency|string¦null|true|none|Валюта|
|type|[TransactionType](#schematransactiontype)|true|none|none|
|description|string¦null|true|none|Описание транзакции|

<h2 id="tocS_TransactionType">TransactionType</h2>
<!-- backwards compatibility -->
<a id="schematransactiontype"></a>
<a id="schema_TransactionType"></a>
<a id="tocStransactiontype"></a>
<a id="tocstransactiontype"></a>

```json
"Credit"

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|string|false|none|none|

#### Enumerated Values

|Property|Value|
|---|---|
|*anonymous*|Credit|
|*anonymous*|Debit|

<h2 id="tocS_TransactionViewModel">TransactionViewModel</h2>
<!-- backwards compatibility -->
<a id="schematransactionviewmodel"></a>
<a id="schema_TransactionViewModel"></a>
<a id="tocStransactionviewmodel"></a>
<a id="tocstransactionviewmodel"></a>

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string",
  "createdAt": "2019-08-24T14:15:22Z",
  "isApply": true
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string(uuid)|true|none|Идентификатор транзакции|
|bankAccountId|string(uuid)|true|none|Идентификатор счета|
|counterpartyBankAccountId|string(uuid)¦null|false|none|Идентификатор счета контрагента|
|amount|number(double)|true|none|Количество денег|
|currency|string¦null|true|none|Валюта|
|type|[TransactionType](#schematransactiontype)|true|none|none|
|description|string¦null|true|none|Описание|
|createdAt|string(date-time)|true|none|Дата создания транзакции|
|isApply|boolean|true|none|Флаг принятия транзакции|

<h2 id="tocS_TransferTransactionCommand">TransferTransactionCommand</h2>
<!-- backwards compatibility -->
<a id="schematransfertransactioncommand"></a>
<a id="schema_TransferTransactionCommand"></a>
<a id="tocStransfertransactioncommand"></a>
<a id="tocstransfertransactioncommand"></a>

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "description": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|bankAccountId|string(uuid)|true|none|Идентификатор счета|
|counterpartyBankAccountId|string(uuid)|true|none|Идентификато счета контрагента|
|amount|number(double)|true|none|Количество денег|
|currency|string¦null|true|none|Валюта|
|description|string¦null|true|none|Описание транзакции|

