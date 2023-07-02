using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace osg {

    public class InteractCommand {
        private Environment environment;
        private NationRepository nationRepository;
        private EventProducer eventProducer;
        private EntityRepository entityRepository;

        public InteractCommand(Environment environment, NationRepository nationRepository, EventProducer eventProducer, EntityRepository entityRepository) {
            this.environment = environment;
            this.nationRepository = nationRepository;
            this.eventProducer = eventProducer;
            this.entityRepository = entityRepository;
        }

        public void execute(Player player) {
            AppleTree tree = environment.getNearestTree(player.getGameObject().transform.position);
            Rock rock = environment.getNearestRock(player.getGameObject().transform.position);
            Pawn pawn = (Pawn) environment.getNearestEntityOfType(player.getGameObject().transform.position, EntityType.PAWN);
            Settlement settlement = (Settlement) environment.getNearestEntityOfType(player.getGameObject().transform.position, EntityType.SETTLEMENT);

            if (settlement != null && Vector3.Distance(player.getGameObject().transform.position, settlement.getGameObject().transform.position) < 5) {
                if (settlement.getCurrentlyPresentEntitiesCount() == 0) {
                    player.getStatus().update("No one is home.");
                    return;
                }
                string listOfPresentPawns = "";
                foreach (EntityId entityId in settlement.getCurrentlyPresentEntities()) {
                    Pawn pawnInSettlement = (Pawn) entityRepository.getEntity(entityId);
                    string pawnName = pawnInSettlement.getName();
                    listOfPresentPawns += pawnName + ", ";
                    if (listOfPresentPawns.Length > 50) {
                        listOfPresentPawns += "...";
                        break;
                    }
                }
                player.getStatus().update("Settlement contains: " + listOfPresentPawns);
            }
            else if (pawn != null && Vector3.Distance(player.getGameObject().transform.position, pawn.getGameObject().transform.position) < 5) {
                Nation pawnsNation = nationRepository.getNation(pawn.getNationId());

                List <string> phrases = generatePhrases(pawnsNation, pawn, player);
                string phrase = phrases[UnityEngine.Random.Range(0, phrases.Count)];
                player.getStatus().update(pawn.getName() + ": \"" + phrase + "\"");
            }
            else if (tree != null && Vector3.Distance(player.getGameObject().transform.position, tree.getGameObject().transform.position) < 5) {
                tree.markForDeletion();
                player.getInventory().transferContentsOfInventory(tree.getInventory());
                player.getStatus().update("Gathered wood from tree.");
            }
            else if (rock != null && Vector3.Distance(player.getGameObject().transform.position, rock.getGameObject().transform.position) < 5) {
                rock.markForDeletion();
                player.getInventory().transferContentsOfInventory(rock.getInventory());
                player.getStatus().update("Gathered stone from rock.");
            }
            else {
                player.getStatus().update("No entities within range to interact with.");
            }
        }

        private List<string> generatePhrases(Nation pawnsNation, Pawn pawn, Player player) {
            List <string> phrases = new List<string>() {
                "Hello!",
                "How are you?",
                "Glory to " + pawnsNation.getName() + "!",
                "What's your name?",
                "I'm " + pawn.getName() + ".",
                "Have you heard of " + pawnsNation.getName() + "?",
                "Nice to meet you!",
                "I'm hungry.",
                "The weather is nice today."
            };
            if (pawnsNation.getNumberOfMembers() > 1) {
                phrases.Add("There are " + pawnsNation.getNumberOfMembers() + " members in " + pawnsNation.getName() + ".");

                if (pawnsNation.getLeaderId() == player.getId()) {
                    phrases.Add("Hey boss!");
                    phrases.Add("How's it going boss?");
                    phrases.Add("What's up boss?");
                    phrases.Add("I'm glad to be a member of " + pawnsNation.getName() + ".");
                    phrases.Add("Please don't kick me out of " + pawnsNation.getName() + ".");
                    phrases.Add("When are we going to build more houses?");
                }
            }

            return phrases;
        }

        private void attemptToSellItemsToPawn(Player player, Pawn pawn) {
            int numWood = player.getInventory().getNumItems(ItemType.WOOD);
            int numStone = player.getInventory().getNumItems(ItemType.STONE);
            int numApples = player.getInventory().getNumItems(ItemType.APPLE);

            int cost = (numWood * 5) + (numStone * 10) + (numApples * 1);

            if (cost == 0) {
                player.getStatus().update("You don't have anything to sell.");
                return;
            }

            if (pawn.getInventory().getNumItems(ItemType.GOLD_COIN) < cost) {
                player.getStatus().update(pawn.getName() + " doesn't have enough gold coins to buy your items.");
                return;
            }

            player.getInventory().removeItem(ItemType.WOOD, numWood);
            player.getInventory().removeItem(ItemType.STONE, numStone);
            player.getInventory().removeItem(ItemType.APPLE, numApples);
            pawn.getInventory().addItem(ItemType.WOOD, numWood);
            pawn.getInventory().addItem(ItemType.STONE, numStone);
            pawn.getInventory().addItem(ItemType.APPLE, numApples);
            player.getInventory().addItem(ItemType.GOLD_COIN, cost);
            pawn.getInventory().removeItem(ItemType.GOLD_COIN, cost);
            player.getStatus().update("Sold " + numWood + " wood, " + numStone + " stone, and " + numApples + " apples to " + pawn.getName() + " for " + cost + " gold coins.");
            return;
        }
    }
}