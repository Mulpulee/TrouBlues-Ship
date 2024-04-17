-- Data.lua
Data = {
    SpyPerPlayer = { 1, 2, 2, 3, 3 },
    ShipRequirements = {
        { 20, 10, 10, 4 },
        { 25, 15, 15, 5 },
        { 30, 20, 20, 6 },
        { 35, 25, 25, 7 },
        { 40, 30, 30, 8 }
    },
    AIRequirements = {
        { 10, 3, 3 },
        { 12, 8, 8 },
        { 15, 13, 13 },
        { 17, 18, 18 },
        { 20, 23, 23 }
    },
    ProvidedItems = { 8, 9, 10, 12, 14 },
    SelectableItems = { 5, 6, 7, 8, 9 },
    FailureProbability = { 20, 50, 80 },
    SearchedItemPercentage = {
        { 25, 28, 31, 0, 13, 3 },
        { 32, 34, 24, 0, 8, 2 },
        { 24, 31, 32, 0, 11, 2 }
    },
    --SearchedItemPercentage = {
    --    { 20, 24, 27, 13, 13, 3 },
    --    { 26, 30, 20, 14, 8, 2 },
    --    { 21, 27, 28, 11, 11, 2 }
    --},
    JobPerPlayer = { 2, 2, 3, 3, 4 },
    EngineerSkillLevel = {
        { 4, 2, 2 },
        { 5, 3, 3 },
        { 6, 4, 4 },
        { 7, 5, 5 },
        { 8, 5, 5 }
    },
    ExpelMilestones = {
        { 100, 100, 100 },
        { 50, 100, 100 },
        { 30, 60, 100 }
    }
}
