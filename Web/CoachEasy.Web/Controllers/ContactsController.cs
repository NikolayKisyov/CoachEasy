namespace CoachEasy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CoachEasy.Common;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Messaging;
    using CoachEasy.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : BaseController
    {
        private const string RedirectedFromContactForm = "RedirectedFromContactForm";
        private readonly IRepository<ContactForm> contactsRepository;
        private readonly IEmailSender emailSender;

        public ContactsController(IRepository<ContactForm> contactsRepository, IEmailSender emailSender)
        {
            this.contactsRepository = contactsRepository;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactFormViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
            var contactFormEntry = new ContactForm
            {
                Name = model.Name,
                Email = model.Email,
                Title = model.Title,
                Content = model.Content,
                Ip = ip,
            };
            await this.contactsRepository.AddAsync(contactFormEntry);
            await this.contactsRepository.SaveChangesAsync();

            await this.emailSender.SendEmailAsync(
                model.Email,
                model.Name,
                GlobalConstants.SystemEmail,
                model.Title,
                model.Content);

            this.TempData[RedirectedFromContactForm] = true;

            return this.RedirectToAction("EmailSent");
        }

        public IActionResult EmailSent()
        {
            if (this.TempData[RedirectedFromContactForm] == null)
            {
                return this.NotFound();
            }

            return this.View();
        }
    }
}
