using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

// В автотестах реализованы:
// - Проверка отображения и корректности основных элементов
// - State-transition (проверка переходов между состояниями)
// - Проверка классов эквивалентности для поля Email
// - В Tear down - реализовано сохранение скриншотов в случае "падения" тестов (по принципу имя метода - который сфэйлился, а так же проверка названия на недопустимые символы)

namespace SeleniumTestsAtPractice
{
    class ParrotNameTests
    {
        public ChromeDriver driver;
        public WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized"); // браузер раскрывается на весь экран
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        // Переменные-локаторы элементов для проверок

        private By text1Locator = By.ClassName("text-1");
        private By titleLocator = By.ClassName("title");
        private By subtitleboldLocator = By.ClassName("subtitle-bold");
        private By formQuestion1Locator = By.XPath("//*[@id=\"form\"]/div[2]");
        private By maleRadioButtonLocator = By.Id("boy");
        private By choiceFirstLocator = By.XPath("//*[@id=\"form\"]/div[3]/label[1]");
        private By femaleRadioButtonLocator = By.Id("girl");
        private By choiceSecondLocator = By.XPath("//*[@id=\"form\"]/div[3]/label[2]");
        private By formQuestion2Locator = By.XPath("//*[@id=\"form\"]/div[4]");
        private By emailInputLocator = By.Name("email");
        private By sendMeButtonLocator = By.Id("sendMe");
        private By resultTextLocator = By.ClassName("result-text");
        private By yourEmailLocator = By.ClassName("your-email");
        private By anotherEmailLocator = By.Id("anotherEmail");
        private By formErrorLocator = By.ClassName("form-error");

        // Корректные данные для проверок элементов страницы

        private string siteUrl = "https://qa-course.kontur.host/selenium-practice/";
        private string pageTitleCorrect = "Тестирование программного обеспечения";
        private string text1Correct = "Тестирование программного обеспечения";
        private string titleCorrect = "Не знаешь как назвать?";
        private string subtitleboldCorrect = "Мы подберём имя для твоего попугайчика!";
        private string formQuestion1Correct = "Кто у тебя?";
        private string choiceFirstCorrect = "мальчик";
        private string choiceSecondCorrect = "девочка";
        private string formQuestion2Correct = "На какой e-mail прислать варианты имён?";
        private string placeholderCorrect = "e-mail";
        private string sendMeButtonTextCorrect = "ПОДОБРАТЬ ИМЯ";
        private string resultMaleTextCorrect = "Хорошо, мы пришлём имя для вашего мальчика на e-mail:";
        private string resultFemaleTextCorrect = "Хорошо, мы пришлём имя для вашей девочки на e-mail:";
        private string anotherEmailTextCorrect = "указать другой e-mail";
        private string formErrorInputTextCorrect = "Введите email";
        private string formErrorIncorrectTextCorrect = "Некорректный email";

        // Переменные для тестов классов эквивалентности

        private string validEmail = "validemail@mail.com";

        private string invalidEmail = "invalidemail";
        private string invalidEmail1 = "@";
        private string invalidEmail2 = "@.";
        private string invalidEmail3 = "@x.";

        // Проверка названия тайтла страницы

        [Test]
        public void ParrotNamingSite_CheckingTitleName_TitleNameIsCorrect()
        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.Title.Equals(pageTitleCorrect), "На странице подбора имени для попугайчика, неверный текст заголовка страницы, при переходе на страницу.");

        }

        // Проверка текста хедера

        [Test]
        public void ParrotNamingSite_HeaderTextDisplayCheck_HeaderIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(text1Locator).Displayed, "На странице подбора имени для попугайчика, текст в хедере не отображается, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_CheckingHeaderText_HeaderTextIsCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(text1Correct, driver.FindElement(text1Locator).Text, "На странице подбора имени для попугайчика, неверный текст в хедере, при переходе на страницу.");
        }

        // Проверка тайтла

        [Test]
        public void ParrotNamingSite_TitleTextDisplayCheck_TitleTextIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(titleLocator).Displayed, "На странице подбора имени для попугайчика, текст заголовка формы не отображается, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_CheckingTitleText_TitleTextIsCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(titleCorrect, driver.FindElement(titleLocator).Text, "На странице подбора имени для попугайчика, неверный текст в заголовке формы, при переходе на страницу.");
        }

        // Проверка саб-тайтла

        [Test]
        public void ParrotNamingSite_SubTitleTextDisplayCheck_SubTitleTextIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(subtitleboldLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, текст подзаголовка формы не отображается, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_CheckingSubTitleText_SubTitleTextIsCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(subtitleboldCorrect, driver.FindElement(subtitleboldLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в подзаголовке формы, при переходе на страницу.");
        }

        // Проверка вопроса в форме (вопрос о поле попугайчика)

        [Test]
        public void ParrotNamingSite_FormGenderQuestionTextDisplayCheck_FormGenderQuestionTextIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(formQuestion1Locator).Displayed, "На странице подбора имени для попугайчика, вопрос в форме(про пол попугайчика) не отображается, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_CheckingFormGenderQuestionText_FormGenderQuestionTextIsCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(formQuestion1Correct, driver.FindElement(formQuestion1Locator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в вопросе формы(про пол попугайчика), при переходе на страницу.");
        }

        // Проверка радио-баттонов

        // Радио-баттон мальчика

        [Test]
        public void ParrotNamingSite_MaleRadioButtonDisplayCheck_MaleRadioButtonIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(maleRadioButtonLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не отображается радио-баттон для пола попугайчика - мальчик, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_MaleRadioButtonLabelDisplayCheck_MaleRadioButtonLabelIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(choiceFirstLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, описание к радио-баттону для пола попугайчика - мальчик, не отображается, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_CheckingMaleRadioButtonLabelText_MaleRadioButtonLabelTextIsCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(choiceFirstCorrect, driver.FindElement(choiceFirstLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в описании к радио-баттону для пола попугайчика - девочка, при переходе на страницу.");
        }

        // Радио-баттон девочки

        [Test]
        public void ParrotNamingSite_FemaleRadioButtonDisplayCheck_FemaleRadioButtonIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(femaleRadioButtonLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не отображается радио-баттон для пола попугайчика - девочка, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_FemaleRadioButtonLabelDisplayCheck_FemaleRadioButtonLabelIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(choiceSecondLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, описание к радио-баттону для пола попугайчика - девочка, не отображается, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_CheckingFemaleRadioButtonLabelText_FemaleRadioButtonLabelTextIsCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(choiceSecondCorrect, driver.FindElement(choiceSecondLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в описании к радио-баттону для пола попугайчика - мальчик, при переходе на страницу.");
        }

        // Проверка вопроса в форме (вопрос о email)

        [Test]
        public void ParrotNamingSite_FormEmailQuestionTextDisplayCheck_FormEmailQuestionTextIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(formQuestion2Locator).Displayed, "На странице подбора имени для попугайчика, вопрос в форме(на какой email прислать) не отображается, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_CheckingFormEmailQuestionText_FormEmailQuestionTextIsCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(formQuestion2Correct, driver.FindElement(formQuestion2Locator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в вопросе формы(на какой email прислать), при переходе на страницу.");
        }

        // Проверка поля ввода email

        [Test]
        public void ParrotNamingSite_FormEmailTextFieldDisplayCheck_FormEmailTextFieldIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(emailInputLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, поле ввода email не отображается, при переходе на страницу.");
        }

        // Проверка плейсхолдера в поле ввода email

        [Test]
        public void ParrotNamingSite_CheckingPlaceholderForEmailTextFieldText_PlaceholderForEmailTextIsCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(placeholderCorrect, driver.FindElement(emailInputLocator).GetAttribute("placeholder"), "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в плейсхолдере в поле ввода email, при переходе на страницу.");
        }

        // Проверка кнопки "Подобрать имя"

        [Test]
        public void ParrotNamingSite_FormEmailSendMeButtonDisplayCheck_FormEmailSendMeButtonIsDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(sendMeButtonLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, кнопка Подобрать имя не отображается, при переходе на страницу.");
        }

        [Test]
        public void ParrotNamingSite_CheckingFormEmailSendMeButtonText_FormEmailSendMeButtonTextIsCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(sendMeButtonTextCorrect, driver.FindElement(sendMeButtonLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в кнопке Подобрать имя, при переходе на страницу.");
        }

        // State-transition 

        // При переходе на страницу радио-баттон Мальчик - выбрана

        [Test]
        public void ParrotNamingSite_CheckingMaleRadioButtonSelectedUponLoading_MaleRadioButtonIsSelected()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.IsTrue(driver.FindElement(maleRadioButtonLocator).Selected, "На странице подбора имени для попугайчика в форме подбора имени, не выбрана радиокнопка мальчик, при переходе на страницу.");
        }

        // При переходе на страницу поле Email - пустое

        [Test]
        public void ParrotNamingSite_EmailTextFieldCheckingEmpty_EmailTextFieldIsEmpty()

        {
            driver.Navigate().GoToUrl(siteUrl);

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, поле ввода email не пустое, при переходе на страницу.");
        }

        // Смена выбора через радио-баттоны (мальчик-девочка)

        [Test]
        public void ParrotNamingSite_ChangingMaleRadioButtonSelectedToFemaleRadioButton_RadioButtonFemaleSelected()

        {
            driver.Navigate().GoToUrl(siteUrl);
            driver.FindElement(femaleRadioButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(femaleRadioButtonLocator).Selected, "На странице подбора имени для попугайчика в форме подбора имени, не сменяется выбор радио-баттона, при клике по радио-баттону девочка");
        }

        // Смена выбора через радио-баттоны (мальчик-девочка-мальчик) 

        [Test]
        public void ParrotNamingSite_ChangingFemaleRadioButtonSelectedToMaleRadioButton_RadioButtonMaleSelected()

        {
            driver.Navigate().GoToUrl(siteUrl);
            driver.FindElement(femaleRadioButtonLocator).Click();
            driver.FindElement(maleRadioButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(maleRadioButtonLocator).Selected, "На странице подбора имени для попугайчика в форме подбора имени, не сменяется выбор радио-баттона, при клике по радио-баттону мальчик из выбранного состояния девочка");
        }

        // Заполнение формы email - при выбранном радио-баттоне мальчик

        [Test]
        public void ParrotNamingSite_FillingEmailTextFieldWhileMaleSelected_EmailIsFilledWithCorrectData()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            Assert.AreEqual(validEmail, driver.FindElement(emailInputLocator).GetAttribute("value"), "На странице подбора имени для попугайчика в форме подбора имени, поле ввода невозможно заполнить, при переходе на страницу и выбранном радио-баттоне мальчик.");
        }

        // Редактирование формы email - при выбранном радио-баттоне мальчик

        [Test]
        public void ParrotNamingSite_ClearingEmailTextFieldWhileMaleSelected_EmailFieldIsCleared()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(emailInputLocator).SendKeys(validEmail);
            driver.FindElement(emailInputLocator).Clear();

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).GetAttribute("value"), "На странице подбора имени для попугайчика в форме подбора имени, поле ввода невозможно удалить данные, при переходе на страницу и выбранном радио-баттоне мальчик.");
        }

        // Редактирование формы email - при выбранном радио-баттоне девочка

        [Test]
        public void ParrotNamingSite_ClearingEmailTextFieldWhileFemaleSelected_EmailFieldIsCleared()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);
            driver.FindElement(emailInputLocator).Clear();

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).GetAttribute("value"), "На странице подбора имени для попугайчика в форме подбора имени, поле ввода невозможно удалить данные, при переходе на страницу и выбранном радио-баттоне девочка.");
        }

        // Отправка формы при радиобаттоне мальчик и валидных данных

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForMaleGender_CorrectResultTextDisplayedForMale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(resultTextLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не отображается result-text, при отправке формы с валидными данными и выбранном радио-баттоне мальчик.");
        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForMaleGender_CorrectResultTextAppearedForMale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(resultMaleTextCorrect, driver.FindElement(resultTextLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в result-text, при отправке формы с валидными данными и выбранном радио-баттоне мальчик.");
        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForMaleGender_CorrectEmailTextDisplayedForMale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(yourEmailLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не отображается email на который отправлено письмо, при отправке формы с валидными данными и выбранном радио-баттоне мальчик.");
        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForMaleGender_EmailMatchTypedEmailForMale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(validEmail, driver.FindElement(yourEmailLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, email на который отправлено письмо не совпадает с введенным, при отправке формы с валидными данными и выбранном радио-баттоне мальчик.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForMaleGender_AnotherEmailLinkDisplayedForMale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(anotherEmailLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, ссылка Указать другой email не отображается, при отправке формы с валидными данными и выбранном радио-баттоне мальчик.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForMaleGender_AnotherEmailLinkTextIsCorrectForMale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(anotherEmailTextCorrect, driver.FindElement(anotherEmailLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в ссылке Указать другой email, при отправке формы с валидными данными и выбранном радио-баттоне мальчик.");

        }

        // Проверка "Указать другой email" при радиобаттоне мальчик 

        [Test]
        public void ParrotNamingSite_ClickingAnotherEmailWithMaleButtonSelected_ResultTextIsNotDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            driver.FindElement(anotherEmailLocator).Click();

            Assert.AreEqual(0, driver.FindElements(resultTextLocator).Count, "На странице подбора имени для попугайчика в форме подбора имени, result-text отображается, после того как нажали Указать другой email.");

        }

        [Test]
        public void ParrotNamingSite_ClickingAnotherEmailWithMaleButtonSelected_EmailInResultNotDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            driver.FindElement(anotherEmailLocator).Click();

            Assert.AreEqual(0, driver.FindElements(yourEmailLocator).Count, "На странице подбора имени для попугайчика в форме подбора имени, email на который отправлено письмо отображается, после того как нажали Указать другой email.");

        }

        [Test]
        public void ParrotNamingSite_ClickingAnotherEmailWithMaleButtonSelected_AnotherEmailLinkIsNotDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            driver.FindElement(anotherEmailLocator).Click();

            Assert.IsFalse(driver.FindElement(anotherEmailLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, ссылка Указать другой email отображается, после того как нажали Указать другой email.");
        }

        [Test]
        public void ParrotNamingSite_ClickingAnotherEmailWithMaleButtonSelected_MaleRadioButtonSelected()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            driver.FindElement(anotherEmailLocator).Click();

            Assert.IsTrue(driver.FindElement(maleRadioButtonLocator).Selected, "На странице подбора имени для попугайчика в форме подбора имени, радио-баттон мальчик не выбран, после того как нажали Указать другой email и при выбранном радио-баттоне мальчик.");

        }

        // Проверка "Введите email" при радиобаттоне мальчик 

        [Test]
        public void ParrotNamingSite_SendingFormWithoutFillingFormForMaleGender_ErrorMessageInputEmailAppeared()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(formErrorLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не появляется предупреждение Введите email, после того как нажали кнопку Подобрать имя с пустым полем email.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithoutFillingFormForMaleGender_ErrorMessageInputEmailTextCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(formErrorInputTextCorrect, driver.FindElement(formErrorLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в предупреждении Введите email, после того как нажали кнопку Подобрать имя с пустым поле email.");

        }

        // Проверка "Некорректный email" при радиобаттоне мальчик 

        [Test]
        public void ParrotNamingSite_SendingFormWithInvalidDataForMaleGender_ErrorMessageIncorrectEmailAppeared()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(invalidEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(formErrorLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не появляется предупреждение Некорректный email, после того как нажали кнопку Подобрать имя с невалидным email поле email.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithInvalidDataForMaleGender_ErrorMessageIncorrectEmailTextCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(maleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(invalidEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(formErrorIncorrectTextCorrect, driver.FindElement(formErrorLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в предупреждении Некорректный email, после того как нажали кнопку Подобрать имя с невалидным email поле email.");

        }



        // Отправка формы при радиобаттоне девочка и валидных данных

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForFemaleGender_CorrectResultTextDisplayedForFemale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(resultTextLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не отображается result-text, при отправке формы с валидными данными и выбранном радио-баттоне девочка.");
        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForFemaleGender_CorrectResultTextAppearedForFemale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(resultFemaleTextCorrect, driver.FindElement(resultTextLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в result-text, при отправке формы с валидными данными и выбранном радио-баттоне девочка.");
        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForFemaleGender_CorrectEmailTextDisplayedForFemale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(yourEmailLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не отображается email на который отправлено письмо, при отправке формы с валидными данными и выбранном радио-баттоне девочка.");
        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForFemaleGender_EmailMatchTypedEmailForFemale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(validEmail, driver.FindElement(yourEmailLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, email на который отправлено письмо не совпадает с введенным, при отправке формы с валидными данными и выбранном радио-баттоне девочка.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForFemaleGender_AnotherEmailLinkDisplayedForFemale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(anotherEmailLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, ссылка Указать другой email не отображается, при отправке формы с валидными данными и выбранном радио-баттоне девочка.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithValidDataForFemaleGender_AnotherEmailLinkTextIsCorrectForFemale()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(anotherEmailTextCorrect, driver.FindElement(anotherEmailLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в ссылке Указать другой email, при отправке формы с валидными данными и выбранном радио-баттоне девочка.");

        }

        // Проверка "Указать другой email" при радиобаттоне девочка

        [Test]
        public void ParrotNamingSite_ClickingAnotherEmailWithFemaleButtonSelected_ResultTextIsNotDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            driver.FindElement(anotherEmailLocator).Click();

            Assert.AreEqual(0, driver.FindElements(resultTextLocator).Count, "На странице подбора имени для попугайчика в форме подбора имени, result-text отображается, после того как нажали Указать другой email.");

        }

        [Test]
        public void ParrotNamingSite_ClickingAnotherEmailWithFemaleButtonSelected_EmailInResultNotDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            driver.FindElement(anotherEmailLocator).Click();

            Assert.AreEqual(0, driver.FindElements(yourEmailLocator).Count, "На странице подбора имени для попугайчика в форме подбора имени, email на который отправлено письмо отображается, после того как нажали Указать другой email.");

        }

        [Test]
        public void ParrotNamingSite_ClickingAnotherEmailWithFemaleButtonSelected_AnotherEmailLinkIsNotDisplayed()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            driver.FindElement(anotherEmailLocator).Click();

            Assert.IsFalse(driver.FindElement(anotherEmailLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, ссылка Указать другой email отображается, после того как нажали Указать другой email.");

        }

        [Test]
        public void ParrotNamingSite_ClickingAnotherEmailWithFemaleButtonSelected_FemaleRadioButtonSelected()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(validEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            driver.FindElement(anotherEmailLocator).Click();

            Assert.IsTrue(driver.FindElement(femaleRadioButtonLocator).Selected, "На странице подбора имени для попугайчика в форме подбора имени, радио-баттон девочка не выбран, после того как нажали Указать другой email и при выбранном радио-баттоне девочка.");

        }


        // Проверка "Введите email" при радиобаттоне девочка

        [Test]
        public void ParrotNamingSite_SendingFormWithoutFillingFormForFemaleGender_ErrorMessageInputEmailAppeared()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(formErrorLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не появляется предупреждение Введите email, после того как нажали кнопку Подобрать имя с пустым полем email.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithoutFillingFormForFemaleGender_ErrorMessageInputEmailTextCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(formErrorInputTextCorrect, driver.FindElement(formErrorLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в предупреждении Введите email, после того как нажали кнопку Подобрать имя с пустым полем email.");

        }

        // Проверка "Некорректный email" при радиобаттоне девочка

        [Test]
        public void ParrotNamingSite_SendingFormWithInvalidDataForFemaleGender_ErrorMessageIncorrectEmailAppeared()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(invalidEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(formErrorLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не появляется предупреждение Некорректный email, после того как нажали кнопку Подобрать имя с невалидным email.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithInvalidDataForFemaleGender_ErrorMessageIncorrectEmailTextCorrect()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(femaleRadioButtonLocator).Click();

            driver.FindElement(emailInputLocator).SendKeys(invalidEmail);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.AreEqual(formErrorIncorrectTextCorrect, driver.FindElement(formErrorLocator).Text, "На странице подбора имени для попугайчика в форме подбора имени, неверный текст в предупреждении Некорректный email, после того как нажали кнопку Подобрать имя с невалидным email.");

        }


        // Проверки валидации поля e-mail

        [Test]
        public void ParrotNamingSite_SendingFormWithInvalidDEmailOne_ErrorMessageIncorrectEmailAppeared()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(emailInputLocator).SendKeys(invalidEmail1);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(formErrorLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не появляется предупреждение Некорректный email, после того как нажали кнопку Подобрать имя с невалидным email.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithInvalidEmailTwo_ErrorMessageIncorrectEmailAppeared()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(emailInputLocator).SendKeys(invalidEmail2);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(formErrorLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не появляется предупреждение Некорректный email, после того как нажали кнопку Подобрать имя с невалидным email.");

        }

        [Test]
        public void ParrotNamingSite_SendingFormWithInvalidEmailThree_ErrorMessageIncorrectEmailAppeared()

        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(emailInputLocator).SendKeys(invalidEmail3);

            driver.FindElement(sendMeButtonLocator).Click();

            Assert.IsTrue(driver.FindElement(formErrorLocator).Displayed, "На странице подбора имени для попугайчика в форме подбора имени, не появляется предупреждение Некорректный email, после того как нажали кнопку Подобрать имя с невалидным email.");

        }

        // Tear-Down 

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                string name_with_special_characters = TestContext.CurrentContext.Test.Name;
                string name_without_special_characters = Regex.Replace(name_with_special_characters, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);

                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(name_without_special_characters + ".png", ScreenshotImageFormat.Png);
            }
            driver.Quit();
        }
    }

}
