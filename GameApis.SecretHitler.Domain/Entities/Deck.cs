namespace GameApis.SecretHitler.Domain.Entities;

public class Deck
{
    private readonly Stack<Card> _cards;

    public int Count => _cards.Count;


    public Deck(Card[] cards)
    {
        _cards = new Stack<Card>(cards);
    }
}
