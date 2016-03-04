using aarhusWebDeveloperCoop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult HandleFormSubmit(ContactForm model) {
            //create a MailMessage object. it uses the System.Net.Mail namespace
            MailMessage message = new MailMessage();

            //set the email where the messages will be sent to
            message.To.Add("juztmejuztme23@gmail.com");

            //take what the user input and put it in the message that's going to be sent to you
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            //for Gmail emails
            using (SmtpClient smtp = new SmtpClient()) {
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

            return RedirectToCurrentUmbracoPage();
        }
    }
}