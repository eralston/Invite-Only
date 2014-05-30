[![Build status](https://ci.appveyor.com/api/projects/status/kxfb0a1t4x70cm5c)](https://ci.appveyor.com/project/eralston/invite-only)

Invite-Only
===========

An simple module for handling invitations in a ASP.Net MVC web application, using Action Filters.

What Is Invite Only?
---------------------
Invite only offers a helpful set of Action Filters and Extensions for identifying people coming into your app from invites, or unique URLs that unauthenticated users can use to authenticate.

Installation
---------------------
Download this repo from Git, open the "Invite Only.sln" file, build the InviteOnly project, then reference the "InviteOnly.dll" assembly in your project.

OR

Install the package from NuGet (https://www.nuget.org/packages/InviteOnly/) using the following package manager console command in Visual Studio:

```
Install-Package InviteOnly
```

Example
---------------------
For an in-depth example on how to use InviteOnly, download this Git repo, open "Invite Only.sln", then build and run the MVC web app. It features a simple example of using the InviteOnly attribute.

Usage
---------------------
To use the invite features, you must: setup persistence via Entity Framework, apply the InviteOnly attribute to an action, generate an invite, and generate an actionlink pointing to the restricted action.

To provide persistence via EntityFramework, any controller that is tagged (or has an action tagged) with an InviteOnlyAttribute instance must provide the right EntityFramework context using two special interfaces.

1) Create a DbContext subclass that implements the IInviteContext interface:

```CSharp
public class ExampleContext : DbContext, IInviteContext
{
    public ExampleContext() : base("name=ExampleContext") { }

    // This property is required by IInviteContext
    public System.Data.Entity.DbSet<InviteOnly.Invite> Invites { get; set; }
}
```

2) Create a Controller subclass that implements the IInviteContextProvider interface:

```CSharp
public class HomeController : Controller, IInviteContextProvider
{
    ExampleContext _context = new ExampleContext();

    // Property required by IInviteContextProvider
    public IInviteContext InviteContext { get { return _context; } }
}
```

3) Create an action and mark it with the InviteOnly attribute, optionally creating a target action in the invite of a request lacking an invite in the querystring is received:

```CSharp
// This action is only allowed if the request has a valid invite code in the querystring
// Otherwise, this will redirect to the Denied action
[InviteOnly(DenyAction = "Denied")]
public ActionResult InviteOnlyAction()
{
    // Pull the current invite for this request and mark it as fulfilled
    Invite invite = this.GetCurrentInvite();
    invite.Fulfilled = true;
    _context.SaveChanges();
    return View();
}

public ActionResult Denied() { return View(); }
```

NOTE: The default value for action, if not specified, is "index". It is also possible to choose a different controller (other than the current controller) to receive the redirected request.

4) To access the action, generate a new Invite model, then pass it to the view:

```CSharp
public ActionResult Index()
{
    // Try to select the first unfulfilled invite in the system
    Invite firstInvite = _context.Invites.Where(i => i.Fulfilled == false).Take(1).SingleOrDefault();
    if (firstInvite == null)
    {
        // If we didn't find one, then make a new one
        firstInvite = Invite.Create(_context);
        _context.SaveChanges();
    }

    // Pass the invite into the view
    return View(firstInvite);
}
```

5) Finally, inside the view CSHTML, use the @Html.ActionLinkWithInvite method to generate an anchor tag with the invite's value as a querystring parameter:

```
@using InviteOnly

@model InviteOnly.Invite

@Html.ActionLinkWithInvite(Model, "Try To Act, With Invite", "InviteOnlyAction", null, new { @Class = "btn btn-default" })
```

You can also generate the link using the normal @Html.ActionLink helper and passing the Invite model's "Value" property as a route value with the name "invite".

```
@Html.ActionLink("Try to Act, With Invite", "InviteOnlyAction", new { invite = Model.Value }, new { @Class = "btn btn-default" })
```

In HTML, either of these methods will result in a link that looks similar to this:

```
<a href="/Home/InviteOnlyAction?invite=97330336303d4074a09b68b6ed3c8ead" class="btn btn-default">Try To Act, With Invite</a>
```
Notice the invite code is just added as a querystring parameter named "invite".

For more examples of usage, please try the example project in the repo.
