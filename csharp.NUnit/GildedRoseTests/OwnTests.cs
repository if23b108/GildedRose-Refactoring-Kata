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