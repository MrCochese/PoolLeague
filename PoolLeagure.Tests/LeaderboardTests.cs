using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
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
            result.ShouldBeEquivalentTo(new[]
            {
                player
            });
        }

        [Fact]
        public void Player_joins_at_bottom_of_leaderboard()
        {
            var player = new Player("Barry");
            var player2 = new Player("Larry");
            var leaderboard = new Leaderboard();
            var result = leaderboard.JoinPlayer(player).JoinPlayer(player2);
            result.ShouldBeEquivalentTo(new[]
            {
                player,
                player2
            });
        }

        [Fact]
        public void Players_swap_position_when_challenge_won()
        {
            var player = new Player("Barry");
            var player2 = new Player("Larry");
            var player3 = new Player("Harry");
            var leaderboard = new Leaderboard();
            var result = leaderboard.JoinPlayer(player)
                                    .JoinPlayer(player2)
                                    .JoinPlayer(player3)
                                    .WinChallenge(player, player2);

            result.ShouldBeEquivalentTo(new []
            {
                player2,
                player,
                player3
            });
        }

        [Fact]
        public void Player_can_challenge_up_to_two_rankings_up()
        {
            var leaderboard = new Leaderboard().JoinPlayer(new Player("Hester")).JoinPlayer(new Player("Chester"))
                .JoinPlayer(new Player("Lester")).JoinPlayer(new Player("Nestor"));

            leaderboard.WhoCanIChallenge(new Player("Nestor")).ShouldBeEquivalentTo(new []
            {
                new Player("Chester"),
                new Player("Lester")
            });
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

        public Leaderboard WinChallenge(Player challengee, Player challenger)
        {
            return new Leaderboard()
            {
                _ranking = _ranking.Select(x => Equals(x, challengee) ? challenger : (Equals(x, challenger) ? challengee : x))
            };
        }

        public IEnumerable<Player> WhoCanIChallenge(Player player)
        {
            return _ranking.TakeWhile(x => !x.Equals(player)).Reverse().Take(2);
        }
    }

    public class Player
    {
        public string Name { get; }

        public Player(string name)
        {
            Name = name;
        }

        protected bool Equals(Player other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Player) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
