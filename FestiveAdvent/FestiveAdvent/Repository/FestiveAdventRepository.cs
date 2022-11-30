using FestiveAdvent.Data;

namespace FestiveAdvent.Repository;

public class FestiveAdventRepository
{
    private readonly FestiveAdventData _festiveAdventData;

    public FestiveAdventRepository(FestiveAdventData festiveAdventData)
    {
        _festiveAdventData = festiveAdventData;
    }

    public List<AdventMessage> GetAdventMessages()
    {
        return _festiveAdventData.AdventMessages;
    }
    
    public AdventMessage GetAdventMessage(int id)
    {
        return _festiveAdventData.AdventMessages.FirstOrDefault(a =>a.Id == id);
    }
    
    public AdventMessage AddAdventMessage(AdventMessage adventMessage)
    {
         _festiveAdventData.AdventMessages.Add(new AdventMessage(){Id=adventMessage.Id,Message = adventMessage.Message});
         return adventMessage;
    }
    
    public AdventMessage UpdateAdventMessages(AdventMessage adventMessage)
    {
        var message = GetAdventMessage(adventMessage.Id);
        message.Message = adventMessage.Message;
        return adventMessage;
    }
    
    public void DeleteAdventMessage(int id)
    {
        var message = GetAdventMessage(id);
        _festiveAdventData.AdventMessages.Remove(message);
    }
}