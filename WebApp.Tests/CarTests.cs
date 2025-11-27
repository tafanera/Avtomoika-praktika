using WebApp.models;
using Xunit;

public class CarTests
{

    [Fact]
    public void Car_Test1()
    {
        var car = new Car
        {
            Id = 1,
            Marka = "Тайота",
            Model = "Камри",
            Number = "Е729КВ"
        };

        Assert.Equal(1, car.Id);
        Assert.Equal("Тайота", car.Marka);
        Assert.Equal("Камри", car.Model);
        Assert.Equal("Е729КВ", car.Number);
    }

    [Fact]
    public void Car_Test2()
    {
        var car = new Car { Marka = "Лада" };
        car.Marka = "БМВ";
        Assert.Equal("БМВ", car.Marka);
    }

    [Fact]
    public void Car_Test3()
    {
        var car = new Car { Model = "X3" };
        car.Model = "X5";
        Assert.Equal("X5", car.Model);
    }

    [Fact]
    public void Car_Test4()
    {
        var car = new Car { Number = "В998МК" };
        car.Number = "М228КС";
        Assert.Equal("М228КС", car.Number);
    }

    [Fact]
    public void Car_Test5()
    {
        var car = new Car
        {
            Id = 10,
            Marka = "Мерседес",
            Model = "Бенз",
            Number = "А777АА"
        };

        Assert.Equal(10, car.Id);
        Assert.Equal("Мерседес", car.Marka);
        Assert.Equal("Бенз", car.Model);
        Assert.Equal("А777АА", car.Number);
    }

    [Fact]
    public void Car_Test6()
    {
        var car = new Car { Marka = null };
        Assert.Null(car.Marka);
    }

    [Fact]
    public void Car_Test7()
    {
        var car = new Car { Model = null };
        Assert.Null(car.Model);
    }

    [Fact]
    public void Car_Test8()
    {
        var car = new Car { Number = "" };
        Assert.Equal("", car.Number);
    }
}
