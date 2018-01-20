using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PoolLeagure.Tests
{
    public class LeaderboardTests
    {
        [Fact]
        public void Player_can_join_leaderboard()
        {
            var player = new Player("Barry");
            var leaderboard = new Leaderboard();
            var result = leaderboard.JoinPlayer(player);
            Assert.Contains(player, result);
        }

        [Fact]
        public void Player_joins_at_bottom_of_leaderboard()
        {
            var player = new Player("Barry");
            var player2 = new Player("Larry");
            var leaderboard = new Leaderboard();
            var result = leaderboard.JoinPlayer(player).JoinPlayer(player2);
            Assert.Contains(player, result);
            Assert.Equal(player2, result.Last());
        }
    }

    public class Leaderboard : IEnumerable<Player>
    {
        private IEnumerable<Player> _ranking = new Player[0];

        public Leaderboard JoinPlayer(Player player)
        {
            return new Leaderboard {_ranking = _ranking.Concat(new[] {player})};
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return _ranking.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Player
    {
        public string Name { get; }

        public Player(string name)
        {
            Name = name;
        }
    }
}
