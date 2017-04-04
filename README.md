# Reflect .NET Token Generator

Usage:

```csharp
using Reflect;

Parameter[] params = new Parameter[2]{
    new Parameter("Region", Parameter.Op.Equals, "Northwest"),
    new Parameter("Name", Parameter.Op.NotEquals, "Jonas"),
}

string signedToken = TokenGenerator.Generate("some-secret-key", params);
```
