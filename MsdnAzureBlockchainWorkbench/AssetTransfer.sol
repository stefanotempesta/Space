contract AssetTransfer is LexingtonBase('AssetTransfer')
{
    enum AssetState { Created, Active, OfferPlaced, PendingInspection, Inspected, Appraised, NotionalAcceptance, BuyerAccepted, SellerAccepted, Accepted, Complete, Terminated }
    address public Owner;
    string public Description;
    uint public AskingPrice;
    AssetState public State;
    
    address public Buyer;
    uint public OfferPrice;
    address public Inspector;
    address public Appraiser;
    
    function AssetTransfer(string description, uint256 price) 
    {
        Owner = msg.sender;
        AskingPrice = price;
        Description = description;
        State = AssetState.Active;
        ContractCreated();
    } 
}