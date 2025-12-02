public class CardModel
{
    public CardData data;
    public bool isMatched;
    public bool isRevealed;

    public CardModel(CardData card)
    {
        data = card;
    }
}