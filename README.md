# Jani Blackjack Analyser

A simple blackjack game supported by card counting and basic strategy. Coded in C# to testing basic strategy and card counting algorithms
for further projects. 

## General Notes

* Card suits didn't implemented. It's an algorithm analys tool, so integer values of cards enough for us.
* Cards are implemented in an array of strings. We convert strings into integer values with `getCardValue` function.

```
 public int getCardValue(String card)
        {
            switch (card)
            {
                case "J": return 10;
                case "Q": return 10;
                case "K": return 10;
                case "A": return 11;
                default: return Convert.ToInt32(card);

            }
        }
```       
(To identifying A's next value (1/11) we use `aceFound` function.)

* Decks are shuffling with **Fisher-Yates Shuffle Algorithm**

### Card Counting Algorithm

It's basically checks every remaining cards on deck and shows a probability to player. (In real life usage of card counting
is counting expended cards. To implementing this, I should have create a new array and keep expended cards in there. But it causes
unnecessary usage of memory. Counting remaining cards is practically produces same probability.)

We count the cards as `GoodCards` and `BadCards` If a card won't cause player's bust, it's counted as good card, 
otherwise it's bad card.

``` 
public void cardCounter()
        {
            numOfGoodCards = 0;
            numOfBadCards = 0;

            int remain = 21 - playerTotal;

            for (int i = 0; i < newDeck.Length; i++)
            {
                if (getCardValue(newDeck[i]) <= remain)
                {
                    numOfGoodCards++;
                }

                else
                {
                    numOfBadCards++;
                }
            }
```             

### Basic Strategy Algorithm

Common Rules: http://www.blackjackinstitute.com/store/images/Basic_Strategy_Chart.jpg

Basic strategy rules implemented to a two-dimensional array. A small part:

            ```
            strategyTable[4, 2] = "stand";
            strategyTable[4, 3] = "stand";
            strategyTable[4, 4] = "stand";
            strategyTable[4, 5] = "hit";
            ```
Firstly, it identifies column and row numbers by checking dealer's up card and player's card total. After than it shows `strategyTable` 's content to player.

```
            switch (dealerHand[0])
            {
                case "2": column = 0; break;
                case "3": column = 1; break;
                case "4": column = 2; break;
                         .
                         .
                         .
```

### Cheat Menu
To enable cheat menu press "Enter" key and type this code

`IKnowWhatIamDoing`

## Contributors

Thanks for your support on this project
* Gizem Ece Yılmaz
* Eda Karabiber
* Yusuf Ceylan
* Cemre Tekpınar
