# DynamicwebGlimpse

Hi everyone, thank you for a nice Dynamicweb Tech Conference 2015. In this repository you find the files I used, as promised. If you have any questions or suggestions let me know.

## Installing Glimpse
Note that the entire Web.config is included on GitHub

1. Download the NuGet named [Glimpse.AspNet](http://www.nuget.org/packages/Glimpse.AspNet/)
2. Use the following glimpse configuration in Web.config
```xml
  <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
    <runtimePolicies>
      <ignoredTypes>
        <!-- Allow Glimpse to be shown remotely if another policy allows it -->
        <add type="Glimpse.AspNet.Policy.LocalPolicy, Glimpse.AspNet" />
      </ignoredTypes>
    </runtimePolicies>
    <tabs>
      <ignoredTypes>
        <!-- Hide these tabs, as they desturb Dynamiwceb because actions occours during serialization -->
        <add type="Glimpse.AspNet.Tab.Session, Glimpse.AspNet" />
        <add type="Glimpse.AspNet.Tab.Cache, Glimpse.AspNet" />
      </ignoredTypes>
    </tabs>
  </glimpse>
```
3. Add the following to appSettings in web.config to avoid any competability issues in Dynamicweb Admin
```xml
<add key="Glimpse:DisableAsyncSupport" value="true" />
```
4. Add your own Glimpse Security Policy (see OurSecurityPolicy.cs as an example)

## Using glimpse with NLog
All you have to do here is to install [Glimpse.NLog](http://www.nuget.org/packages/Glimpse.NLog/) and everything that hits NLog is shown. The advantage is also that you do not have to add any rules or targets in NLog for this behaviour. Note that you should not update to the newest Common.Logging as there are some compatability issues as Dynamicweb currently uses the Common.Logging version 2.*.

## Using glimpse with ADO.NET
Start by installing [Glimpse.Ado](http://www.nuget.org/packages/Glimpse.Ado/). If you use a previous version of Dynamicweb than 8.6.0.4, the easiest way is to create a static class in your applicaion, as seen below, that can provide you with an instance of IDbConnection. If you use Dependency Injection, you should use this instead of a static class. 
```csharp
public static class Database
{
	public static IDbConnection CreateConnection()
    {
    	// Note that we want to try to cast it to GlimpseDbConnection first.
    	// This allows us to avoid any complications if we later on decides to implement a version using
    	// the Dynamicweb.IDatabaseConnectionProvider, where we can override the IDbConnection in an earlier stage.
      var conn = Dynamicweb.Database.CreateConnection();
    	return conn as GlimpseDbConnection ?? new GlimpseDbConnection((DbConnection)Dynamicweb.Database.CreateConnection());
    }
}
```
In Dynamicweb 8.6.0.4 you  get the option to implement the interface IDatabaseConnectionProvider, you can see an example of this in DynamicwebConnectionProvider.cs. Not that in that example I wrapped the GlimpseDbConnection and GlimpseDbCommand in a castable class so you can explicit cast to SqlConnection for SQL specific behaviours e.g. Bulk inserts.

