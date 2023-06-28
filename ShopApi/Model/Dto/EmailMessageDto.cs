namespace ShopApi.Dto
{
    public class EmailMessageDto
    {
        public string To { get; set; } = String.Empty; //Адрес электронной почты получателя
        public string Subject { get; set; } = String.Empty; //Строка темы
        public string Content { get; set; } = String.Empty; // Тело письма
    }

}