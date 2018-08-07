using System;
using System.Collections.Generic;

static class Variables
{
    public static int NUM_INGREDIENTS = 3;
    public static int NUM_UTENSILS = 2;

    // Used in CustomerGenerator
    public static int MAX_CUSTOMERS = 4;
    public static float CUSTOMER_OFFSET = 1.5f;

    public static float INGREDIENT_OFFSET = 3f;
    public static float UTENSIL_OFFSET = 3f;

    //to use when searching for the front sign of a restaurant
    public static string RESTAURANT_SPRITE_STRING = "Front_";

    //the max amount of chefs and waiters that can be in any given restaurant at a time
    public static int MAX_CHEFS_IN_RESTAURANT = 1;
    public static int MAX_WAITERS_IN_RESTAURANT = 3;
}
