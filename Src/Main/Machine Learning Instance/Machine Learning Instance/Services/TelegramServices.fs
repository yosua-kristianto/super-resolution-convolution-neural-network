namespace Services

open System.Net.Http;

module TelegramServices = 

    (*
      SendMessage

      This function will send message through Telegram chat to destinated channel. 
    *)
    let SendMessage (message: string) =
        let httpClient = new HttpClient();

        // Request generation
        let uri = "https://api.telegram.org/bot:id/sendMessage?chat_id=chat:id&text=" + message;

        // @async Just send the message
        httpClient.PostAsync(uri, null);

