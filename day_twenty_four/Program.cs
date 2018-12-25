using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_twenty_four {
    public class team {
        public team_type team_Type { get; set; }
        public List<unit> units { get; set; }
    }

    public class unit {
        public int count { get; set; }
        public int hit_points { get; set; }
        public List<dmg_type> weak_to { get; set; }
        public List<dmg_type> immune_to { get; set; }
        public int damage { get; set; }
        public dmg_type dmg_Type { get; set; }
        public int init { get; set; }
        public int effective_power { get; set; }
        public unit target { get; set; }
        public bool targeted { get; set; }
        public int potential_damage { get; set; }
        public void calc_potential_damage (dmg_type atk_type, int unit_count, int damage) {
            if (immune_to.Contains (atk_type)) {
                this.potential_damage = 0;
            } else if (this.weak_to.Contains (atk_type)) {
                this.potential_damage = 2 * (unit_count * damage);
            } else {
                this.potential_damage = unit_count * damage;
            }
        }

        public void calc_effective_power () {
            this.effective_power = this.count * this.damage;
        }

        public void reset () {
            this.potential_damage = 0;
            this.target = null;
            this.targeted = false;
            this.calc_effective_power ();
        }
    }

    public enum dmg_type {
        Fire,
        Slashing,
        Bludgeoning,
        Cold,
        Radiation
    }

    public enum team_type {
        Immune,
        Infection
    }
    class Program {
        static void Main (string[] args) {
            var team_immune = init_immune ();
            var team_infection = init_infection ();

            team_immune.units.ForEach (e => e.calc_effective_power ());
            team_infection.units.ForEach (e => e.calc_effective_power ());
            bool attack = true;
            while (attack) {
                attack = false;
                //target selection
                foreach (var unit in team_immune.units.Where (e => e.count > 0).OrderByDescending (e => e.effective_power)) {
                    team_infection.units.ForEach (e => e.calc_potential_damage (unit.dmg_Type, unit.count, unit.damage));

                    var target = team_infection.units.Where (e => !e.targeted).OrderByDescending (e => (e.potential_damage, e.effective_power, e.init)).FirstOrDefault ();

                    if (target == null) continue;

                    target.targeted = true;
                    unit.target = target;
                }

                foreach (var unit in team_infection.units.Where (e => e.count > 0).OrderByDescending (e => e.effective_power)) {
                    team_immune.units.ForEach (e => e.calc_potential_damage (unit.dmg_Type, unit.count, unit.damage));

                    var target = team_immune.units.Where (e => !e.targeted).OrderByDescending (e => (e.potential_damage, e.effective_power, e.init)).FirstOrDefault ();

                    if (target == null) continue;

                    target.targeted = true;
                    unit.target = target;
                }

                for (int i = 0; i < team_immune.units.Where (e => e.target != null).Count() + team_infection.units.Where (e => e.target != null).Count(); i++)
                {
                    var attacker = team_immune.units.Where (e => e.target != null).Union(team_infection.units.Where (e => e.target != null)).OrderByDescending(e=>e.init).First();

                    var dies = attacker.target.potential_damage / attacker.target.hit_points;
                    attacker.target.count = Math.Max (0, attacker.target.count - dies);
                    if (dies > 0) {
                        attack = true;
                    }

                    attacker.target = null;
                
                }

                team_immune.units.ForEach (e => e.reset ());
                team_infection.units.ForEach (e => e.reset ());
            }

            // 16325
            // 6787
            Console.WriteLine ("Part 1:");
            Console.WriteLine (team_immune.units.Sum (e => e.count) + team_infection.units.Sum (e => e.count));
        }

        public static team init_immune () {
            var team_immune = new team () {
                team_Type = team_type.Immune,
                units = new List<unit> ()
            };

            team_immune.units.Add (new unit {
                count = 504,
                    hit_points = 1697,
                    weak_to = new List<dmg_type> () { dmg_type.Fire },
                    immune_to = new List<dmg_type> () { dmg_type.Slashing },
                    damage = 28,
                    dmg_Type = dmg_type.Fire,
                    init = 4
            });

            team_immune.units.Add (new unit {
                count = 7779,
                    hit_points = 6919,
                    weak_to = new List<dmg_type> () { dmg_type.Bludgeoning },
                    immune_to = new List<dmg_type> (),
                    damage = 7,
                    dmg_Type = dmg_type.Cold,
                    init = 2
            });

            team_immune.units.Add (new unit {
                count = 7913,
                    hit_points = 13214,
                    weak_to = new List<dmg_type> () { dmg_type.Fire, dmg_type.Cold },
                    immune_to = new List<dmg_type> (),
                    damage = 12,
                    dmg_Type = dmg_type.Slashing,
                    init = 14
            });

            team_immune.units.Add (new unit {
                count = 1898,
                    hit_points = 3721,
                    weak_to = new List<dmg_type> () { dmg_type.Bludgeoning },
                    immune_to = new List<dmg_type> (),
                    damage = 16,
                    dmg_Type = dmg_type.Cold,
                    init = 20
            });

            team_immune.units.Add (new unit {
                count = 843,
                    hit_points = 3657,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> () { dmg_type.Slashing },
                    damage = 41,
                    dmg_Type = dmg_type.Cold,
                    init = 17
            });

            team_immune.units.Add (new unit {
                count = 8433,
                    hit_points = 3737,
                    weak_to = new List<dmg_type> () { dmg_type.Bludgeoning },
                    immune_to = new List<dmg_type> () { dmg_type.Radiation },
                    damage = 3,
                    dmg_Type = dmg_type.Bludgeoning,
                    init = 8
            });

            team_immune.units.Add (new unit {
                count = 416,
                    hit_points = 3760,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> () { dmg_type.Fire, dmg_type.Radiation },
                    damage = 64,
                    dmg_Type = dmg_type.Radiation,
                    init = 3
            });

            team_immune.units.Add (new unit {
                count = 5654,
                    hit_points = 1858,
                    weak_to = new List<dmg_type> () { dmg_type.Fire },
                    immune_to = new List<dmg_type> (),
                    damage = 2,
                    dmg_Type = dmg_type.Cold,
                    init = 6
            });

            team_immune.units.Add (new unit {
                count = 2050,
                    hit_points = 8329,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> () { dmg_type.Radiation, dmg_type.Cold },
                    damage = 36,
                    dmg_Type = dmg_type.Radiation,
                    init = 12
            });

            team_immune.units.Add (new unit {
                count = 4130,
                    hit_points = 3560,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> (),
                    damage = 8,
                    dmg_Type = dmg_type.Bludgeoning,
                    init = 13
            });

            return team_immune;
        }

        public static team init_infection () {
            var team_infection = new team () {
                team_Type = team_type.Infection,
                units = new List<unit> ()
            };

            team_infection.units.Add (new unit {
                count = 442,
                    hit_points = 35928,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> (),
                    damage = 149,
                    dmg_Type = dmg_type.Bludgeoning,
                    init = 11
            });

            team_infection.units.Add (new unit {
                count = 61,
                    hit_points = 42443,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> () { dmg_type.Radiation },
                    damage = 1289,
                    dmg_Type = dmg_type.Slashing,
                    init = 7
            });

            team_infection.units.Add (new unit {
                count = 833,
                    hit_points = 6874,
                    weak_to = new List<dmg_type> () { dmg_type.Slashing },
                    immune_to = new List<dmg_type> (),
                    damage = 14,
                    dmg_Type = dmg_type.Bludgeoning,
                    init = 15
            });

            team_infection.units.Add (new unit {
                count = 1832,
                    hit_points = 61645,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> (),
                    damage = 49,
                    dmg_Type = dmg_type.Fire,
                    init = 9
            });

            team_infection.units.Add (new unit {
                count = 487,
                    hit_points = 26212,
                    weak_to = new List<dmg_type> () { dmg_type.Fire },
                    immune_to = new List<dmg_type> (),
                    damage = 107,
                    dmg_Type = dmg_type.Bludgeoning,
                    init = 16
            });

            team_infection.units.Add (new unit {
                count = 2537,
                    hit_points = 18290,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> () { dmg_type.Cold, dmg_type.Slashing, dmg_type.Fire },
                    damage = 11,
                    dmg_Type = dmg_type.Fire,
                    init = 19
            });

            team_infection.units.Add (new unit {
                count = 141,
                    hit_points = 14369,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> () { dmg_type.Bludgeoning },
                    damage = 178,
                    dmg_Type = dmg_type.Radiation,
                    init = 5
            });

            team_infection.units.Add (new unit {
                count = 3570,
                    hit_points = 34371,
                    weak_to = new List<dmg_type> (),
                    immune_to = new List<dmg_type> (),
                    damage = 18,
                    dmg_Type = dmg_type.Radiation,
                    init = 10
            });

            team_infection.units.Add (new unit {
                count = 5513,
                    hit_points = 60180,
                    weak_to = new List<dmg_type> () { dmg_type.Radiation, dmg_type.Fire },
                    immune_to = new List<dmg_type> (),
                    damage = 16,
                    dmg_Type = dmg_type.Slashing,
                    init = 1
            });

            team_infection.units.Add (new unit {
                count = 2378,
                    hit_points = 20731,
                    weak_to = new List<dmg_type> () { dmg_type.Bludgeoning },
                    immune_to = new List<dmg_type> (),
                    damage = 17,
                    dmg_Type = dmg_type.Radiation,
                    init = 18
            });

            return team_infection;
        }
    }
}