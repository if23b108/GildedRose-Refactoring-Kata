using GildedRoseKata;
using GildedRoseTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRoseTests
{

    //Helper for all tests so that every test has exact condition
    public static class TestHelper
    {
        public static Item Make(string name, int sellIn, int quality) =>
            new() { Name = name, SellIn = sellIn, Quality = quality };

        public static Item AfterOneDay(Item item)
        {
            var app = new GildedRose(new List<Item> { item });
            app.UpdateQuality();          // tick once
            return item;          // return reference
        }

        public static Item AfterOneDay(string name, int sellIn, int quality) =>
            AfterOneDay(Make(name, sellIn, quality));
    }

}
//Test stragtegie: 

//first normmal items without special rules
//testing if items behave to the basic rules of ->


#region Standard Item Test

[TestFixture]
public class StandardItemTests
{
    //fiexed test numbers for all items
    [TestCase(10, 20, 9, 19)]
    [TestCase(1, 7, 0, 6)]


    //-> item decreases normally when the sell date is in the future
    
    public void DegradesByOne_BeforeSellDate(int startSellIn, int startQuality, int expectedSellIn, int expectedQuality)
    {
        var item = TestHelper.AfterOneDay("+5 Dexterity Vest", startSellIn, startQuality);
        Assert.Multiple(() =>
        {
            Assert.That(item.SellIn, Is.EqualTo(expectedSellIn));
            Assert.That(item.Quality, Is.EqualTo(expectedQuality));
        });
    }


    //-> items decreases more when the sell date is over 

    [Test]

    public void DegradesTwiceAsFast_AfterSellDate()
    {
        var item = TestHelper.AfterOneDay("Elixir of the Mongoose", 0, 10);
        Assert.That(item.Quality, Is.EqualTo(8));
    }


    //-> the item isnnt allowed to have a quality less that 0
    [Test]
    public void Quality_Never_Negative()
    {
        var item = TestHelper.AfterOneDay("Elixir of the Mongoose", 5, 0);
        Assert.That(item.Quality, Is.EqualTo(0));
    }
}

#endregion

//now special item aged brie with some special rules
//testing if the item behaves to the rules of ->

#region Aged Brie

[TestFixture]
public class AgedBrieTests
{

    //-> Aged Brie increases in quality as it gets older
    [Test]
    public void IncreasesQuality_BeforeSellDate()
    {
        var item = TestHelper.AfterOneDay("Aged Brie", 2, 0);
        Assert.Multiple(() =>
        {
            Assert.That(item.SellIn, Is.EqualTo(1));
            Assert.That(item.Quality, Is.EqualTo(1));
        });
    }

    //-> Aged Brie increases in quality twice as fast after the sell date
    [Test]
    public void IncreasesTwiceAsFast_AfterSellDate()
    {
        var item = TestHelper.AfterOneDay("Aged Brie", 0, 48);
        Assert.That(item.Quality, Is.EqualTo(50)); // capped at 50
    }


    //-> Aged Brie cant have a quality more than 50
    [Test]
    public void Quality_Never_Above50()
    {
        var item = TestHelper.AfterOneDay("Aged Brie", 5, 50);
        Assert.That(item.Quality, Is.EqualTo(50));
    }
}

#endregion


//now special item Backstage Pass with some special rules
//testing if the item behaves to the rules of ->

#region Backstage Passes

[TestFixture]
public class BackstagePassTests
{
    private const string Backstage = "Backstage passes to a TAFKAL80ETC concert";


    //-> Backstage Passes increase normal when the concert is 10 days or more away

    [Test]
    public void IncreaseByOne_When_MoreThan10Days()
    {
        var item = TestHelper.AfterOneDay(Backstage, 11, 20);
        Assert.That(item.Quality, Is.EqualTo(21));
    }

    //-> increase double when the concert is in 6-10 days

    [Test]
    public void IncreaseByTwo_When_Between6And10Days()
    {
        var item = TestHelper.AfterOneDay(Backstage, 10, 45);
        Assert.That(item.Quality, Is.EqualTo(47));
    }


    //-> increase triple when the concert is in 1-5 days

    [Test]
    public void IncreaseByThree_When_Between1And5Days()
    {
        var item = TestHelper.AfterOneDay(Backstage, 5, 46);
        Assert.That(item.Quality, Is.EqualTo(49)); 
    }


    //-> qualiy 0 when the concert is over

    [Test]
    public void Quality_DropsToZero_AfterConcert()
    {
        var item = TestHelper.AfterOneDay(Backstage, 0, 40);
        Assert.That(item.Quality, Is.EqualTo(0));
    }


    // -> same as all
    [Test]
    public void Quality_Never_Above50()
    {
        var item = TestHelper.AfterOneDay(Backstage, 9, 50);
        Assert.That(item.Quality, Is.EqualTo(50));
    }
}

#endregion

//now special item Sulfuras that never changes
//testing if the item behaves to the rules of ->


#region Sulfuras

[TestFixture]
public class SulfurasTests
{

    // -> Sulfuras, Hand of Ragnaros doenst change in sell time or quality and is allowed to have a qualitiy of mor than 50
    [Test]
    public void NeverChanges()
    {
        var item = TestHelper.AfterOneDay("Sulfuras, Hand of Ragnaros", 0, 80);
        Assert.Multiple(() =>
        {
            Assert.That(item.SellIn, Is.EqualTo(0));
            Assert.That(item.Quality, Is.EqualTo(80));
        });
    }



}

#endregion