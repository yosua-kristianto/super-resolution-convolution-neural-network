namespace Services

open System.Net.Http;

module TelegramServices = 
    let SendMessage (message: string) =
        let httpClient = new HttpClient();

        // Request generation
        let uri = "https://api.telegram.org/bot:id/sendMessage?chat_id=chat:id&text=" + message;

        // Just send the message
        httpClient.PostAsync(uri, null);

