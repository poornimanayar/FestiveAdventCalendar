using FestiveAdvent.Data;
using FestiveAdvent.Repository;
using FestiveAdventApi;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace FestiveAdvent.Services;

public class FestiveAdventApiService : FestiveAdventService.FestiveAdventServiceBase
{
    private readonly FestiveAdventRepository _festiveAdventRepository;


    public FestiveAdventApiService(FestiveAdventRepository festiveAdventRepository)
    {
        _festiveAdventRepository = festiveAdventRepository;
    }

    public override Task<ListReply> ListMessages(Empty request, ServerCallContext context)
    {
        var items = _festiveAdventRepository.GetAdventMessages();
        
        var listReply = new ListReply();
        var messageList = items.Select(item => new AdventMessageReply() { Id = item.Id,Message = item.Message }).ToList();
        listReply.AdventMessages.AddRange(messageList);
        return Task.FromResult(listReply);
    }

    public override Task<AdventMessageReply> GetMessage(GetAdventMessageRequest request, ServerCallContext context)
    {
        var message = _festiveAdventRepository.GetAdventMessage(request.Id);
        return Task.FromResult(new AdventMessageReply() { Id = message.Id, Message = message.Message });
    }

    public override Task<AdventMessageReply> CreateMessage(CreateAdventMessageRequest request, ServerCallContext context)
    {
        var messageToCreate = new AdventMessage()
        {
            Id=request.Id,
            Message = request.Message
        };

        var createdMessage = _festiveAdventRepository.AddAdventMessage(messageToCreate);

        var reply = new AdventMessageReply() { Id = createdMessage.Id, Message = createdMessage.Message};

        return Task.FromResult(reply);
    }

    public override Task<AdventMessageReply> UpdateMessage(UpdateAdventMessageRequest request, ServerCallContext context)
    {
        var existingMessage = _festiveAdventRepository.GetAdventMessage(request.Id);

        if (existingMessage == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Advent message not found"));
        }
        
        var messageToUpdate = new AdventMessage()
        {
            Id=request.Id,
            Message = request.Message
        };

        var createdMessage = _festiveAdventRepository.UpdateAdventMessages(messageToUpdate);

        var reply = new AdventMessageReply() { Id = createdMessage.Id, Message = createdMessage.Message};

        return Task.FromResult(reply);
    }

    public override Task<Empty> DeleteMessage(DeleteAdventMessageRequest request, ServerCallContext context)
    {
        var existingMessage = _festiveAdventRepository.GetAdventMessage(request.Id);

        if (existingMessage == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Advent message not found"));
        }
        
        _festiveAdventRepository.DeleteAdventMessage(request.Id);
        
        return Task.FromResult(new Empty());
    }
}
