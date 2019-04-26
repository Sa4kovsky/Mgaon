using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Website.Additional;
using Website.Models;
using Website.Models.Context;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        IHostingEnvironment _appEnvironment;

        private readonly IStringLocalizer<HomeController> _localizer;
        private ContextNews db;

        public HomeController(ContextNews context, IHostingEnvironment appEnvironment, IStringLocalizer<HomeController> localizer)
        {
            _appEnvironment = appEnvironment;
            _localizer = localizer;
            db = context;
        }

        Models.FileMode file;
        
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);

        }

        public async Task<IActionResult> Index()
        {
            var news = db.News.Where(p => p.Language == _localizer["Language"].Value.ToString()).Where(p => p.DateStart.Year == DateTime.Now.Year && p.DateStart.Month == DateTime.Now.Month);
            return View(await news.ToListAsync());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page. asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [ asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd  asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd  asdd ddd dddd ddd dddd hiohui hiuh ii uhuiuhi hiu iu hi hi hi hi hi no h hi h [  kj ksbndbaisd as du;as dash ashd ohaos ho hoi h ho ho h o ho hoh ks hd0a] dd dddd dddd ddd ddd dddd ddddd ddd ddddd dddd dd dddd ddd dddddd ddddd ddd ddddd d ddddd dddddd ddddd";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Message() //обращение
        {
            return View(new Person());
        }

        public IActionResult MessageLegal() //обращение юр лиц
        {
            return View(new LegalPerson());
        }

        [HttpPost]
        public async Task<IActionResult> Message(IFormFile uploadedFile, LegalPerson person)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, System.IO.FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                file = new Models.FileMode { Name = uploadedFile.FileName, Path = path };
            }
            SendEmailAsync(person, file, "Электронное обращение");
            ViewData["Message"] = _localizer["SentMessage"];
            return View();
        }

        public IActionResult Feedback() // сообщение
        {
            return View(new Person());
        }

        [HttpPost]
        public IActionResult Feedback(LegalPerson person)
        {
            SendEmailAsync(person, file, "Обратная связь");
            ViewData["Message"] = _localizer["SentFeedback"];
            return View();
        }

        public async void SendEmailAsync(LegalPerson person, Models.FileMode file, string messaga)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("notification.prereg@mgaon.by", person.Name);
            // кому отправляем
            MailAddress to = new MailAddress("notification.prereg@mgaon.by");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = messaga;
            // текст письма
            m.Body = "<html><body> <br>" + "<br>" + messaga + @"
                          <br>Наименование и (или) адрес организации либо должность лица - "+ person.Designation + @"
                          <br>Полное наименование юридического лица (обязательно) - " + person.NameLegal + @"
                          <br>ФИО  -  " + person.Name+ @"
                          <br>Адрес места жительства (места пребывания) - " + person.Address + @"
                          <br>E-mail - " + person.Email + @"
                          <br>Контактный телефон - " + person.Phone + @"                                                                                              
                          <br>
                          <br>Сообщение - " + person.Message + @" </body></html>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            if (file != null)
            {
                m.Attachments.Add(new Attachment(_appEnvironment.WebRootPath + file.Path));
            }
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("notification.prereg@mgaon.by", "4rfvBGT5");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }

        public IActionResult ChairmanLine() // горячая линия
        {
            return View();
        }

        public IActionResult EnterpriseAdministration() 
        {
            return View();
        }

        public IActionResult AdministrationProc() // административные процедуры
        {
            return View();
        }

        public IActionResult PersonalReception() //личный прием
        {
            return View();
        }

        public IActionResult Advertising() // реклама
        {
            return View();
        }


        public IActionResult ADMProcedures() // Зак-во об адм процедурах
        {
            return View();
        }

        public IActionResult ColCentr() // Кол-центр
        {
            return View();
        }

        public IActionResult WebCamers() // камеры
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
