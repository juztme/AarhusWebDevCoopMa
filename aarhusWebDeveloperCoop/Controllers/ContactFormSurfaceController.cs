using aarhusWebDeveloperCoop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;


namespace aarhusWebDeveloperCoop.Controllers
{
  public class ContactFormSurfaceController : Umbraco.Web.Mvc.SurfaceController
  {
    // GET: ContactFormSurface
    public ActionResult Index()
    {
      return PartialView("ContactForm", new ContactForm());
    }

    [HttpPost]
    public ActionResult HandleFormSubmit(ContactForm model)
    {

      if (!ModelState.IsValid) { return CurrentUmbracoPage(); }

      //create a MailMessage object. it uses the System.Net.Mail namespace
      MailMessage message = new MailMessage();

      //set the email where the messages will be sent to
      message.To.Add("juztmejuztme23@gmail.com");

      //take what the user input and put it in the message that's going to be sent to you
      message.Subject = model.Subject;
      message.From = new MailAddress(model.Email, model.Name);
      message.Body = model.Message;

      //for Gmail emails
      using (SmtpClient smtp = new SmtpClient())
      {
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.UseDefaultCredentials = false;
        smtp.EnableSsl = true;
        smtp.Host = "smtp.gmail.com";
        smtp.Port = 587;
        smtp.Credentials = new System.Net.NetworkCredential("juztmejuztme23@gmail.com", "vyfrxyanifccwdvk");
        smtp.EnableSsl = true;

        //send mail
        smtp.Send(message);
      }

      TempData["success"] = true;

      //CAN'T APPLY THIS CODE BECAUSE IF I TRY TO MAKE A MESSAGES DOCUMENT TYPE, IT BREAKS THE ENTIRE THING

      //add the received mail as a document that can be seen in the backoffice area
      //the 3 parameters taken by the CreateContent method are: the name of the new element, the new element's parentId in the content tree
      //and the last one is the contentTypeAlias of the element
      IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "userMessage");
      comment.SetValue("yourName", model.Name);
      comment.SetValue("email", model.Email);
      comment.SetValue("subject", model.Subject);
      comment.SetValue("message", model.Message);

      //save
      Services.ContentService.Save(comment);

      //save and publish; if you uncomment it, it's going to publish it, but if you go to the published message page in the browser, it's going to show you a blank page
      //Services.ContentService.SaveAndPublishWithStatus(comment);

      return RedirectToCurrentUmbracoPage();
    }
  }
}