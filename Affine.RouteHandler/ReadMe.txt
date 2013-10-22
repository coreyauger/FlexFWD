**** NOTE 
* IF YOU UPGRADED DNN *
You need to replace the DotNetNuke.dll in the bin ...


Spent a lot of time configuring these...
1) Make sure your Web.Config <modules> section looks like this
 <modules runAllManagedModulesForAllRequests="true">

 2) Make sure to add these as well
 <remove name="UrlRoutingModule" />
      <add name="UrlRoutingModule" type="System.Web.Routing.UrlRoutingModule, System.Web.Routing, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" />

3) To handlers you add this:
<add name="UrlRoutingHandler" preCondition="integratedMode" verb="*" path="UrlRouting.axd" type="System.Web.HttpForbiddenHandler, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />