namespace NorwayWalks.API.Models.DTO
{
    public class WalksDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }

        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        //Navigation Property
        public Region Region { get; set; }
        public WalkDifficultyDTO WalkDifficulty { get; set; }
    }
}
