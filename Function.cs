using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json;

// Assembly attribute to enable Lambda JSON serialization
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WifiQrLambdaProcessor;

public class Function
{
    /// <summary>
    /// Lambda function handler that processes SQS messages
    /// </summary>
    /// <param name="sqsEvent">SQS event containing messages</param>
    /// <param name="context">Lambda execution context</param>
    public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processing {sqsEvent.Records.Count} messages from SQS");

        foreach (var record in sqsEvent.Records)
        {
            await ProcessMessageAsync(record, context);
        }
    }

    /// <summary>
    /// Processes a single SQS message
    /// </summary>
    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"Processing message: {message.MessageId}");
            context.Logger.LogInformation($"Message body: {message.Body}");

            // Deserialize the WiFi QR created message
            var wifiMessage = JsonSerializer.Deserialize<WifiQrCreatedMessage>(
                message.Body,
                new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            if (wifiMessage == null)
            {
                context.Logger.LogWarning($"Failed to deserialize message: {message.MessageId}");
                return;
            }

            // Log the WiFi QR code details
            context.Logger.LogInformation(
                $"WiFi QR Code Created - " +
                $"ID: {wifiMessage.WifiId}, " +
                $"SSID: {wifiMessage.Ssid}, " +
                $"Encryption: {wifiMessage.Encryption}, " +
                $"CreatedAt: {wifiMessage.CreatedAt}, " +
                $"CreatedBy: {wifiMessage.CreatedBy}");

            // Process the message - Add your business logic here
            await ProcessWifiQrCodeAsync(wifiMessage, context);

            context.Logger.LogInformation($"Successfully processed message: {message.MessageId}");
        }
        catch (JsonException jsonEx)
        {
            context.Logger.LogError($"JSON deserialization error for message {message.MessageId}: {jsonEx.Message}");
            // Don't throw - message will be deleted from queue
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error processing message {message.MessageId}: {ex.Message}");
            // Throw to keep message in queue for retry
            throw;
        }
    }

    /// <summary>
    /// Processes the WiFi QR code creation event
    /// </summary>
    private async Task ProcessWifiQrCodeAsync(WifiQrCreatedMessage message, ILambdaContext context)
    {
        // Add your business logic here
        await SendEmailNotificationAsync(message, context);
        await UpdateAnalyticsAsync(message, context);
        await LogAuditTrailAsync(message, context);
    }

    private async Task SendEmailNotificationAsync(WifiQrCreatedMessage message, ILambdaContext context)
    {
        // TODO: Implement email notification using Amazon SES
        context.Logger.LogInformation($"Sending email notification for WiFi: {message.Ssid}");
        await Task.CompletedTask;
    }

    private async Task UpdateAnalyticsAsync(WifiQrCreatedMessage message, ILambdaContext context)
    {
        // TODO: Implement analytics update (e.g., CloudWatch metrics, DynamoDB)
        context.Logger.LogInformation($"Updating analytics for WiFi ID: {message.WifiId}");
        await Task.CompletedTask;
    }

    private async Task LogAuditTrailAsync(WifiQrCreatedMessage message, ILambdaContext context)
    {
        context.Logger.LogInformation(
            $"Audit: WiFi QR created by {message.CreatedBy} at {message.CreatedAt}");
        await Task.CompletedTask;
    }
}

