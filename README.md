# reflect-dotnet

This is a client for the [Reflect API](https://reflect.io/docs/).

## Generating an Authentication Token

Here's an example of how to generate an encrypted authentication token:

```csharp
var accessKey = "d232c1e5-6083-4aa7-9042-0547052cc5dd";
var secretKey = "74678a9b-685c-4c14-ac45-7312fe29de06";

var parameter = new Reflect.Tokens.Parameter("My Field", Parameter.Op.Equal, "My Value");

var token = new TokenBuilder(accessKey)
    .SetAttribute("user-id", 1234)
    .SetAttribute("user-name", "Billy Bob")
    .AddParameter(parameter)
    .Build(secretKey);
```
