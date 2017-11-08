using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using ApsMind.Olympiad.Portal.Providers;
using ApsMind.Olympiad.Framework;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Twitter;
using Microsoft.Web.WebPages.OAuth;
using Microsoft.AspNet.Identity.Owin;
namespace ApsMind.Olympiad.Portal
{
    public partial class Startup
    {
        static Startup()
        {
            PublicClientId = "self";

            UserManagerFactory = () => new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static Func<UserManager<IdentityUser>> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);


            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            (
           
            ));
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            // clientId: "",
            // clientSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
                
            //    ClientId = "363721670242-rv6ee16ksonn86pm7e8b0gmfgurmmc2g.apps.googleusercontent.com",
            //    ClientSecret = "Pm-OH7zPSctmuEs9jJ_ZTyAV"
            //});


            OAuthWebSecurity.RegisterFacebookClient(
              appId: "130759670881383",
              appSecret: "0384dad2d533eea4e3b2c0bb51f8c5c6");
            
            // for www.silverzonementor.com
            //OAuthWebSecurity.RegisterFacebookClient(
            //   appId: "134221413843536",
            //   appSecret: "66b3729d433c8a4feec915f92eb02698");
           
            //OAuthWebSecurity.RegisterTwitterClient(
            //  consumerKey: "r44D4meCerzJFhBtz8Qzckagm",
            //  consumerSecret: "mEj10tenTlJuaD1PepozgcT0RarbPVJJxtRL61xoaJWTH1KbDL");
            
            //OAuthWebSecurity.RegisterGoogleClient();
           
            //OAuthWebSecurity.RegisterLinkedInClient(
            //    consumerKey: "81k9hfbwm6389x",
            //    consumerSecret: "O1dSemLiRgflIj3h"
            //    );

            
          
            
        }
    }
}
