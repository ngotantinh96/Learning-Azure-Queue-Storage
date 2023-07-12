// Get the connection string from app settings
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string connectionString = "Storage Account Connection String";

// Queue Storage name
string queueName = "tn204queue-storage";

// Instantiate a QueueClient which will be used to create and manipulate the queue
QueueClient queueClient = new QueueClient(connectionString, queueName);

// Create the queue
queueClient.CreateIfNotExists();

if (queueClient.Exists())
{
    // Send a message to the queue
    string message = "Hello, world!";
    queueClient.SendMessage(message);


    Console.WriteLine("Message is sent.");
    Console.WriteLine("Press any key to continue");
    Console.ReadKey();

    // Peek at the next message
    PeekedMessage[] peekedMessages = queueClient.PeekMessages();

    foreach (var peekedMessage in peekedMessages) 
    {
        Console.WriteLine($"Message ID {peekedMessage.MessageId }| content: {peekedMessage.Body.ToString}");
    }

    Console.WriteLine("Press any key to continue");
    Console.ReadKey();

    // Get the message from the queue
    QueueMessage[] receivedMessages = queueClient.ReceiveMessages();

    // Update the message contents
    foreach(var receivedMessage in receivedMessages) 
    {
        Console.WriteLine($"Updating message: {receivedMessage.MessageId}");
        queueClient.UpdateMessage(receivedMessage.MessageId, 
            receivedMessage.PopReceipt, 
            $"Updated contents {receivedMessage.MessageId}",
            TimeSpan.FromSeconds(60.0)  // Make it invisible for another 60 seconds
        );
    }

    Console.WriteLine("Press any key to continue");
    Console.ReadKey();

    QueueProperties properties = queueClient.GetProperties();

    // Retrieve the cached approximate message count.
    int cachedMessagesCount = properties.ApproximateMessagesCount;

    // Display number of messages.
    Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");

    Console.WriteLine("Press any key to continue");
    Console.ReadKey();

    // Get the next message
    QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

    // Process (i.e. print) the message in less than 30 seconds
    Console.WriteLine($"Dequeued message: '{retrievedMessage[0].Body}'");

    // Delete the message
    queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);

    Console.WriteLine("Press any key to continue");
    Console.ReadKey();

    // Delete the queue
    Console.WriteLine("Deleting queue...");
    queueClient.Delete();

    Console.WriteLine("Press any key to continue");
    Console.ReadKey();
}