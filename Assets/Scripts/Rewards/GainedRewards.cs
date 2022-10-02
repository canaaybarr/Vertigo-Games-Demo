using System.Collections.Generic;

namespace Rewards
{
    public class GainedRewards : Singleton<GainedRewards>
    {
        public List<Reward> gainedRewards = new List<Reward>();
    }
}
