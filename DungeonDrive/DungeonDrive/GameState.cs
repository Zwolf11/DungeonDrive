using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
using System.IO;

namespace DungeonDrive
{
    public class GameState : State
    {
        public Hero hero;
        public Room room;
        public Item[][] inventory = new Item[5][];
        public Font font = new Font("Arial", 12);
        public int size = 40;
        public String graveyard = "C:\\graveyard";
        private SoundPlayer saveSound = new SoundPlayer(Properties.Resources.level_up);
        public String currentRoom = "C:\\";
        public String pastRoom;
        public String[] adjectives = { "Abandoned", "Able", "Absolute", "Adorable", "Adventurous", "Academic", "Acceptable", "Acclaimed", "Accomplished", "Accurate", "Aching", "Acidic", "Acrobatic", "Active", "Actual", "Adept", "Admirable", "Admired", "Adolescent", "Adorable", "Adored", "Advanced", "Afraid", "Affectionate", "Aged", "Aggravating", "Aggressive", "Agile", "Agitated", "Agonizing", "Agreeable", "Ajar", "Alarmed", "Alarming", "Alert", "Alienated", "Alive", "All", "Altruistic", "Amazing", "Ambitious", "Ample", "Amused", "Amusing", "Anchored", "Ancient", "Angelic", "Angry", "Anguished", "Animated", "Annual", "Another", "Antique", "Anxious", "Any", "Apprehensive", "Appropriate", "Apt", "Arctic", "Arid", "Aromatic", "Artistic", "Ashamed", "Assured", "Astonishing", "Athletic", "Attached", "Attentive", "Attractive", "Austere", "Authentic", "Authorized", "Automatic", "Avaricious", "Average", "Aware", "Awesome", "Awful", "Awkward", "Babyish", "Bad", "Back", "Baggy", "Bare", "Barren", "Basic", "Beautiful", "Belated", "Beloved", "Beneficial", "Better", "Best", "Bewitched", "Big", "Big-Hearted", "Biodegradable", "Bite-Sized", "Bitter", "Black", "Black-And-White", "Bland", "Blank", "Blaring", "Bleak", "Blind", "Blissful", "Blond", "Blue", "Blushing", "Bogus", "Boiling", "Bold", "Bony", "Boring", "Bossy", "Both", "Bouncy", "Bountiful", "Bowed", "Brave", "Breakable", "Brief", "Bright", "Brilliant", "Brisk", "Broken", "Bronze", "Brown", "Bruised", "Bubbly", "Bulky", "Bumpy", "Buoyant", "Burdensome", "Burly", "Bustling", "Busy", "Buttery", "Buzzing", "Calculating", "Calm", "Candid", "Canine", "Capital", "Carefree", "Careful", "Careless", "Caring", "Cautious", "Cavernous", "Celebrated", "Charming", "Cheap", "Cheerful", "Cheery", "Chief", "Chilly", "Chubby", "Circular", "Classic", "Clean", "Clear", "Clear-Cut", "Clever", "Close", "Closed", "Cloudy", "Clueless", "Clumsy", "Cluttered", "Coarse", "Cold", "Colorful", "Colorless", "Colossal", "Comfortable", "Common", "Compassionate", "Competent", "Complete", "Complex", "Complicated", "Composed", "Concerned", "Concrete", "Confused", "Conscious", "Considerate", "Constant", "Content", "Conventional", "Cooked", "Cool", "Cooperative", "Coordinated", "Corny", "Corrupt", "Costly", "Courageous", "Courteous", "Crafty", "Crazy", "Creamy", "Creative", "Creepy", "Criminal", "Crisp", "Critical", "Crooked", "Crowded", "Cruel", "Crushing", "Cuddly", "Cultivated", "Cultured", "Cumbersome", "Curly", "Curvy", "Cute", "Cylindrical", "Damaged", "Damp", "Dangerous", "Dapper", "Daring", "Darling", "Dark", "Dazzling", "Dead", "Deadly", "Deafening", "Dear", "Dearest", "Decent", "Decimal", "Decisive", "Deep", "Defenseless", "Defensive", "Defiant", "Deficient", "Definite", "Definitive", "Delayed", "Delectable", "Delicious", "Delightful", "Delirious", "Demanding", "Dense", "Dental", "Dependable", "Dependent", "Descriptive", "Deserted", "Detailed", "Determined", "Devoted", "Different", "Difficult", "Digital", "Diligent", "Dim", "Dimpled", "Dimwitted", "Direct", "Disastrous", "Discrete", "Disfigured", "Disgusting", "Disloyal", "Dismal", "Distant", "Downright", "Dreary", "Dirty", "Disguised", "Dishonest", "Dismal", "Distant", "Distinct", "Distorted", "Dizzy", "Dopey", "Doting", "Double", "Downright", "Drab", "Drafty", "Dramatic", "Dreary", "Droopy", "Dry", "Dual", "Dull", "Dutiful", "Each", "Eager", "Earnest", "Early", "Easy", "Easy-Going", "Ecstatic", "Edible", "Educated", "Elaborate", "Elastic", "Elated", "Elderly", "Electric", "Elegant", "Elementary", "Elliptical", "Embarrassed", "Embellished", "Eminent", "Emotional", "Empty", "Enchanted", "Enchanting", "Energetic", "Enlightened", "Enormous", "Enraged", "Entire", "Envious", "Equal", "Equatorial", "Essential", "Esteemed", "Ethical", "Euphoric", "Even", "Evergreen", "Everlasting", "Every", "Evil", "Exalted", "Excellent", "Exemplary", "Exhausted", "Excitable", "Excited", "Exciting", "Exotic", "Expensive", "Experienced", "Expert", "Extraneous", "Extroverted", "Extra-Large", "Extra-Small", "Fabulous", "Failing", "Faint", "Fair", "Faithful", "Fake", "False", "Familiar", "Famous", "Fancy", "Fantastic", "Far", "Faraway", "Far-Flung", "Far-Off", "Fast", "Fat", "Fatal", "Fatherly", "Favorable", "Favorite", "Fearful", "Fearless", "Feisty", "Feline", "Female", "Feminine", "Few", "Fickle", "Filthy", "Fine", "Finished", "Firm", "First", "Firsthand", "Fitting", "Fixed", "Flaky", "Flamboyant", "Flashy", "Flat", "Flawed", "Flawless", "Flickering", "Flimsy", "Flippant", "Flowery", "Fluffy", "Fluid", "Flustered", "Focused", "Fond", "Foolhardy", "Foolish", "Forceful", "Forked", "Formal", "Forsaken", "Forthright", "Fortunate", "Fragrant", "Frail", "Frank", "Frayed", "Free", "French", "Fresh", "Frequent", "Friendly", "Frightened", "Frightening", "Frigid", "Frilly", "Frizzy", "Frivolous", "Front", "Frosty", "Frozen", "Frugal", "Fruitful", "Full", "Fumbling", "Functional", "Funny", "Fussy", "Fuzzy", "Gargantuan", "Gaseous", "General", "Generous", "Gentle", "Genuine", "Giant", "Giddy", "Gigantic", "Gifted", "Giving", "Glamorous", "Glaring", "Glass", "Gleaming", "Gleeful", "Glistening", "Glittering", "Gloomy", "Glorious", "Glossy", "Glum", "Golden", "Good", "Good-Natured", "Gorgeous", "Graceful", "Gracious", "Grand", "Grandiose", "Granular", "Grateful", "Grave", "Gray", "Great", "Greedy", "Green", "Gregarious", "Grim", "Grimy", "Gripping", "Gross", "Grotesque", "Grouchy", "Grounded", "Growing", "Growling", "Grown", "Grubby", "Gruesome", "Grumpy", "Guilty", "Gullible", "Gummy", "Hairy", "Half", "Handmade", "Handsome", "Handy", "Happy", "Happy-Go-Lucky", "Hard", "Hard-To-Find", "Harmful", "Harmless", "Harmonious", "Harsh", "Hasty", "Hateful", "Haunting", "Healthy", "Heartfelt", "Hearty", "Heavenly", "Heavy", "Hefty", "Helpful", "Helpless", "Hidden", "Hideous", "High", "High-Level", "Hilarious", "Hoarse", "Hollow", "Homely", "Honest", "Honorable", "Honored", "Hopeful", "Horrible", "Hospitable", "Hot", "Huge", "Humble", "Humiliating", "Humming", "Humongous", "Hungry", "Hurtful", "Husky", "Icky", "Icy", "Ideal", "Idealistic", "Identical", "Idle", "Idiotic", "Idolized", "Ignorant", "Ill", "Illegal", "Ill-Fated", "Ill-Informed", "Illiterate", "Illustrious", "Imaginary", "Imaginative", "Immaculate", "Immaterial", "Immediate", "Immense", "Impassioned", "Impeccable", "Impartial", "Imperfect", "Imperturbable", "Impish", "Impolite", "Important", "Impossible", "Impractical", "Impressionable", "Impressive", "Improbable", "Impure", "Inborn", "Incomparable", "Incompatible", "Incomplete", "Inconsequential", "Incredible", "Indelible", "Inexperienced", "Indolent", "Infamous", "Infantile", "Infatuated", "Inferior", "Infinite", "Informal", "Innocent", "Insecure", "Insidious", "Insignificant", "Insistent", "Instructive", "Insubstantial", "Intelligent", "Intent", "Intentional", "Interesting", "Internal", "International", "Intrepid", "Ironclad", "Irresponsible", "Irritating", "Itchy", "Jaded", "Jagged", "Jam-Packed", "Jaunty", "Jealous", "Jittery", "Joint", "Jolly", "Jovial", "Joyful", "Joyous", "Jubilant", "Judicious", "Juicy", "Jumbo", "Junior", "Jumpy", "Juvenile", "Kaleidoscopic", "Keen", "Key", "Kind", "Kindhearted", "Kindly", "Klutzy", "Knobby", "Knotty", "Knowledgeable", "Knowing", "Known", "Kooky", "Kosher", "Lame", "Lanky", "Large", "Last", "Lasting", "Late", "Lavish", "Lawful", "Lazy", "Leading", "Lean", "Leafy", "Left", "Legal", "Legitimate", "Light", "Lighthearted", "Likable", "Likely", "Limited", "Limp", "Limping", "Linear", "Lined", "Liquid", "Little", "Live", "Lively", "Livid", "Loathsome", "Lone", "Lonely", "Long", "Long-Term", "Loose", "Lopsided", "Lost", "Loud", "Lovable", "Lovely", "Loving", "Low", "Loyal", "Lucky", "Lumbering", "Luminous", "Lumpy", "Lustrous", "Luxurious", "Mad", "Made-Up", "Magnificent", "Majestic", "Major", "Male", "Mammoth", "Married", "Marvelous", "Masculine", "Massive", "Mature", "Meager", "Mealy", "Mean", "Measly", "Meaty", "Medical", "Mediocre", "Medium", "Meek", "Mellow", "Melodic", "Memorable", "Menacing", "Merry", "Messy", "Metallic", "Mild", "Milky", "Mindless", "Miniature", "Minor", "Minty", "Miserable", "Miserly", "Misguided", "Misty", "Mixed", "Modern", "Modest", "Moist", "Monstrous", "Monthly", "Monumental", "Moral", "Mortified", "Motherly", "Motionless", "Mountainous", "Muddy", "Muffled", "Multicolored", "Mundane", "Murky", "Mushy", "Musty", "Muted", "Mysterious", "Naive", "Narrow", "Nasty", "Natural", "Naughty", "Nautical", "Near", "Neat", "Necessary", "Needy", "Negative", "Neglected", "Negligible", "Neighboring", "Nervous", "New", "Next", "Nice", "Nifty", "Nimble", "Nippy", "Nocturnal", "Noisy", "Nonstop", "Normal", "Notable", "Noted", "Noteworthy", "Novel", "Noxious", "Numb", "Nutritious", "Nutty", "Obedient", "Obese", "Oblong", "Oily", "Oblong", "Obvious", "Occasional", "Odd", "Oddball", "Offbeat", "Offensive", "Official", "Old", "Old-Fashioned", "Only", "Open", "Optimal", "Optimistic", "Opulent", "Orange", "Orderly", "Organic", "Ornate", "Ornery", "Ordinary", "Original", "Other", "Our", "Outlying", "Outgoing", "Outlandish", "Outrageous", "Outstanding", "Oval", "Overcooked", "Overdue", "Overjoyed", "Overlooked", "Palatable", "Pale", "Paltry", "Parallel", "Parched", "Partial", "Passionate", "Past", "Pastel", "Peaceful", "Peppery", "Perfect", "Perfumed", "Periodic", "Perky", "Personal", "Pertinent", "Pesky", "Pessimistic", "Petty", "Phony", "Physical", "Piercing", "Pink", "Pitiful", "Plain", "Plaintive", "Plastic", "Playful", "Pleasant", "Pleased", "Pleasing", "Plump", "Plush", "Polished", "Polite", "Political", "Pointed", "Pointless", "Poised", "Poor", "Popular", "Portly", "Posh", "Positive", "Possible", "Potable", "Powerful", "Powerless", "Practical", "Precious", "Present", "Prestigious", "Pretty", "Precious", "Previous", "Pricey", "Prickly", "Primary", "Prime", "Pristine", "Private", "Prize", "Probable", "Productive", "Profitable", "Profuse", "Proper", "Proud", "Prudent", "Punctual", "Pungent", "Puny", "Pure", "Purple", "Pushy", "Putrid", "Puzzled", "Puzzling", "Quaint", "Qualified", "Quarrelsome", "Quarterly", "Queasy", "Querulous", "Questionable", "Quick", "Quick-Witted", "Quiet", "Quintessential", "Quirky", "Quixotic", "Quizzical", "Radiant", "Ragged", "Rapid", "Rare", "Rash", "Raw", "Recent", "Reckless", "Rectangular", "Ready", "Real", "Realistic", "Reasonable", "Red", "Reflecting", "Regal", "Regular", "Reliable", "Relieved", "Remarkable", "Remorseful", "Remote", "Repentant", "Required", "Respectful", "Responsible", "Repulsive", "Revolving", "Rewarding", "Rich", "Rigid", "Right", "Ringed", "Ripe", "Roasted", "Robust", "Rosy", "Rotating", "Rotten", "Rough", "Round", "Rowdy", "Royal", "Rubbery", "Rundown", "Ruddy", "Rude", "Runny", "Rural", "Rusty", "Sad", "Safe", "Salty", "Same", "Sandy", "Sane", "Sarcastic", "Sardonic", "Satisfied", "Scaly", "Scarce", "Scared", "Scary", "Scented", "Scholarly", "Scientific", "Scornful", "Scratchy", "Scrawny", "Second", "Secondary", "Second-Hand", "Secret", "Self-Assured", "Self-Reliant", "Selfish", "Sentimental", "Separate", "Serene", "Serious", "Serpentine", "Several", "Severe", "Shabby", "Shadowy", "Shady", "Shallow", "Shameful", "Shameless", "Sharp", "Shimmering", "Shiny", "Shocked", "Shocking", "Shoddy", "Short", "Short-Term", "Showy", "Shrill", "Shy", "Sick", "Silent", "Silky", "Silly", "Silver", "Similar", "Simple", "Simplistic", "Sinful", "Single", "Sizzling", "Skeletal", "Skinny", "Sleepy", "Slight", "Slim", "Slimy", "Slippery", "Slow", "Slushy", "Small", "Smart", "Smoggy", "Smooth", "Smug", "Snappy", "Snarling", "Sneaky", "Sniveling", "Snoopy", "Sociable", "Soft", "Soggy", "Solid", "Somber", "Some", "Spherical", "Sophisticated", "Sore", "Sorrowful", "Soulful", "Soupy", "Sour", "Spanish", "Sparkling", "Sparse", "Specific", "Spectacular", "Speedy", "Spicy", "Spiffy", "Spirited", "Spiteful", "Splendid", "Spotless", "Spotted", "Spry", "Square", "Squeaky", "Squiggly", "Stable", "Staid", "Stained", "Stale", "Standard", "Starchy", "Stark", "Starry", "Steep", "Sticky", "Stiff", "Stimulating", "Stingy", "Stormy", "Straight", "Strange", "Steel", "Strict", "Strident", "Striking", "Striped", "Strong", "Studious", "Stunning", "Stupendous", "Stupid", "Sturdy", "Stylish", "Subdued", "Submissive", "Substantial", "Subtle", "Suburban", "Sudden", "Sugary", "Sunny", "Super", "Superb", "Superficial", "Superior", "Supportive", "Sure-Footed", "Surprised", "Suspicious", "Svelte", "Sweaty", "Sweet", "Sweltering", "Swift", "Sympathetic", "Tall", "Talkative", "Tame", "Tan", "Tangible", "Tart", "Tasty", "Tattered", "Taut", "Tedious", "Teeming", "Tempting", "Tender", "Tense", "Tepid", "Terrible", "Terrific", "Testy", "Thankful", "That", "These", "Thick", "Thin", "Third", "Thirsty", "This", "Thorough", "Thorny", "Those", "Thoughtful", "Threadbare", "Thrifty", "Thunderous", "Tidy", "Tight", "Timely", "Tinted", "Tiny", "Tired", "Torn", "Total", "Tough", "Traumatic", "Treasured", "Tremendous", "Tragic", "Trained", "Tremendous", "Triangular", "Tricky", "Trifling", "Trim", "Trivial", "Troubled", "True", "Trusting", "Trustworthy", "Trusty", "Truthful", "Tubby", "Turbulent", "Twin", "Ugly", "Ultimate", "Unacceptable", "Unaware", "Uncomfortable", "Uncommon", "Unconscious", "Understated", "Unequaled", "Uneven", "Unfinished", "Unfit", "Unfolded", "Unfortunate", "Unhappy", "Unhealthy", "Uniform", "Unimportant", "United", "Unkempt", "Unknown", "Unlawful", "Unlined", "Unlucky", "Unnatural", "Unpleasant", "Unrealistic", "Unripe", "Unruly", "Unselfish", "Unsightly", "Unsteady", "Unsung", "Untidy", "Untimely", "Untried", "Untrue", "Unused", "Unusual", "Unwelcome", "Unwieldy", "Unwilling", "Unwitting", "Unwritten", "Upbeat", "Upright", "Upset", "Urban", "Usable", "Used", "Useful", "Useless", "Utilized", "Utter", "Vacant", "Vague", "Vain", "Valid", "Vapid", "Variable", "Vast", "Velvety", "Venerated", "Vengeful", "Verifiable", "Vibrant", "Vicious", "Victorious", "Vigilant", "Vigorous", "Villainous", "Violet", "Violent", "Virtual", "Virtuous", "Visible", "Vital", "Vivacious", "Vivid", "Voluminous", "Wan", "Warlike", "Warm", "Warmhearted", "Warped", "Wary", "Wasteful", "Watchful", "Waterlogged", "Watery", "Wavy", "Wealthy", "Weak", "Weary", "Webbed", "Wee", "Weekly", "Weepy", "Weighty", "Weird", "Welcome", "Well-Documented", "Well-Groomed", "Well-Informed", "Well-Lit", "Well-Made", "Well-Off", "Well-To-Do", "Well-Worn", "Wet", "Which", "Whimsical", "Whirlwind", "Whispered", "White", "Whole", "Whopping", "Wicked", "Wide", "Wide-Eyed", "Wiggly", "Wild", "Willing", "Wilted", "Winding", "Windy", "Winged", "Wiry", "Wise", "Witty", "Wobbly", "Woeful", "Wonderful", "Wooden", "Woozy", "Wordy", "Worldly", "Worn", "Worried", "Worrisome", "Worse", "Worst", "Worthless", "Worthwhile", "Worthy", "Wrathful", "Wretched", "Writhing", "Wrong", "Wry", "Yawning", "Yearly", "Yellow", "Yellowish", "Young", "Youthful", "Yummy", "Zany", "Zealous", "Zigzag" };
        
        public enum AtkStyle
        {
            Basic,
            Flame,
            Frozen
        }

        public GameState(MainForm form, bool load) : base(form)
        {
            hero = new Hero(this, 0, 0);
            for (int i = 0; i < inventory.Length; i++)
                inventory[i] = new Item[5];

            if (load)
                loadGame();
            else
            {
                room = new Room(this, "C:\\");
                inventory[0][0] = randomItem();
            }
        }

        public Item randomItem()
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            switch(rand.Next(6))
            {
                case 0:
                    return new Weapon(this);
                case 1:
                    return new Shield(this);
                case 2:
                    return new Helmet(this);
                case 3:
                    return new Armor(this);
                case 4:
                    return new Legs(this);
                case 5:
                    switch(rand.Next(3))
                    {
                        case 0:
                            return new SmallPotion(this);
                        case 1:
                            return new MediumPotion(this);
                        case 2:
                            return new LargePotion(this);
                    }
                    break;
            }

            return null;
        }

        public bool tryPickupItem(Item item)
        {
            if(item is Helmet && hero.helmet == null)
            {
                hero.helmet = (Helmet)item;
                return true;
            }
            else if (item is Armor && hero.armor == null)
            {
                hero.armor = (Armor)item;
                return true;
            }
            else if (item is Legs && hero.legs == null)
            {
                hero.legs = (Legs)item;
                return true;
            }
            else if (item is Shield && hero.shield == null)
            {
                hero.shield = (Shield)item;
                return true;
            }
            else if (item is Weapon && hero.weapon == null)
            {
                hero.weapon = (Weapon)item;
                hero.equipWeapon((Weapon)item);
                return true;
            }

            for(int j=0;j<inventory[0].Length;j++)
                for(int i=0;i<inventory.Length;i++)
                    if(inventory[i][j] == null)
                    {
                        inventory[i][j] = item;
                        return true;
                    }

            return false;
        }

        public void saveGame()
        {
            List<String> save = new List<String>();

            if (pastRoom != null) save.Add(pastRoom);
            else save.Add("NULL");

            save.Add(room.currentRoom);

            save.Add("" + hero.x);
            save.Add("" + hero.y);
            save.Add("" + hero.level);
            save.Add("" + hero.exp);
            save.Add("" + hero.full_hp);
            save.Add("" + hero.hp);
            save.Add("" + hero.atk_dmg);
            save.Add("" + hero.atk_speed);

            if (hero.helmet == null) save.Add("NULL");
            else save.Add(hero.helmet.name + "_" + hero.helmet.defense);
            if (hero.armor == null) save.Add("NULL");
            else save.Add(hero.armor.name + "_" + hero.armor.defense);
            if (hero.legs == null) save.Add("NULL");
            else save.Add(hero.legs.name + "_" + hero.legs.defense);
            if (hero.shield == null) save.Add("NULL");
            else save.Add(hero.shield.name + "_" + hero.shield.defense);
            if (hero.weapon == null) save.Add("NULL");
            else save.Add(hero.weapon.name + "_" + hero.weapon.damage + "_" + hero.weapon.ranged + "_" + hero.weapon.atk_speed + "_" + hero.weapon.proj_speed + "_" + hero.weapon.proj_range + "_" + hero.weapon.powerSec + "_" + hero.weapon.powerFac + "_" + (int)hero.weapon.style);

            for(int j=0;j<inventory[0].Length;j++)
                for(int i=0;i<inventory.Length;i++)
                {
                    if (inventory[i][j] == null)
                        save.Add("NULL");
                    else
                    {
                        if(inventory[i][j] is Helmet)
                        {
                            Helmet helmet = (Helmet)inventory[i][j];
                            save.Add("HELMET_" + helmet.name + "_" + helmet.defense);
                        }
                        else if (inventory[i][j] is Armor)
                        {
                            Armor armor = (Armor)inventory[i][j];
                            save.Add("ARMOR_" + armor.name + "_" + armor.defense);
                        }
                        else if (inventory[i][j] is Legs)
                        {
                            Legs legs = (Legs)inventory[i][j];
                            save.Add("LEGS_" + legs.name + "_" + legs.defense);
                        }
                        else if (inventory[i][j] is Shield)
                        {
                            Shield shield = (Shield)inventory[i][j];
                            save.Add("SHIELD_" + shield.name + "_" + shield.defense);
                        }
                        else if (inventory[i][j] is Weapon)
                        {
                            Weapon weapon = (Weapon)inventory[i][j];
                            save.Add("WEAPON_" + weapon.name + "_" + weapon.damage + "_" + weapon.ranged + "_" + weapon.atk_speed + "_" + weapon.proj_speed + "_" + weapon.proj_range + "_" + weapon.powerSec + "_" + weapon.powerFac + "_" + (int)weapon.style);
                        }
                        else
                        {
                            save.Add(inventory[i][j].name);
                        }
                    }
                }

            File.WriteAllLines("save", save);

            saveSound.Play();
        }

        private void loadGame()
        {
            String[] loadFile = File.ReadAllLines("save");

            if(loadFile[0] != "NULL")
                pastRoom = loadFile[0];
            room = new Room(this, loadFile[1]);

            hero.x = double.Parse(loadFile[2]);
            hero.y = double.Parse(loadFile[3]);
            hero.level = int.Parse(loadFile[4]);
            hero.exp = double.Parse(loadFile[5]);
            hero.full_hp = double.Parse(loadFile[6]);
            hero.hp = double.Parse(loadFile[7]);
            hero.atk_dmg = double.Parse(loadFile[8]);
            hero.atk_speed = double.Parse(loadFile[9]);

            String[] helmet = loadFile[10].Split('_');
            if (helmet[0] != "NULL")
                hero.helmet = new Helmet(this, helmet[0], int.Parse(helmet[1]));
            String[] armor = loadFile[11].Split('_');
            if (armor[0] != "NULL")
                hero.armor = new Armor(this, armor[0], int.Parse(armor[1]));
            String[] legs = loadFile[12].Split('_');
            if (legs[0] != "NULL")
                hero.legs = new Legs(this, legs[0], int.Parse(legs[1]));
            String[] shield = loadFile[13].Split('_');
            if (shield[0] != "NULL")
                hero.shield = new Shield(this, shield[0], int.Parse(shield[1]));
            String[] weapon = loadFile[14].Split('_');
            if (weapon[0] != "NULL")
                hero.weapon = new Weapon(this, weapon[0], int.Parse(weapon[1]), bool.Parse(weapon[2]), double.Parse(weapon[3]), double.Parse(weapon[4]), int.Parse(weapon[5]), double.Parse(weapon[6]), double.Parse(weapon[7]), int.Parse(weapon[8]) );

            int loc = 15;
            for (int j = 0; j < inventory[0].Length; j++)
                for (int i = 0; i < inventory.Length; i++)
                {
                    String[] item = loadFile[loc++].Split('_');

                    if(item[0] != "NULL")
                    {
                        if (item[0] == "HELMET")
                            inventory[i][j] = new Helmet(this, item[1], int.Parse(item[2]));
                        else if (item[0] == "ARMOR")
                            inventory[i][j] = new Armor(this, item[1], int.Parse(item[2]));
                        else if (item[0] == "LEGS")
                            inventory[i][j] = new Legs(this, item[1], int.Parse(item[2]));
                        else if (item[0] == "SHIELD")
                            inventory[i][j] = new Shield(this, item[1], int.Parse(item[2]));
                        else if (item[0] == "WEAPON")
                            inventory[i][j] = new Weapon(this, item[1], int.Parse(item[2]), bool.Parse(item[3]), double.Parse(item[4]), double.Parse(item[5]), int.Parse(item[6]), double.Parse(item[7]), double.Parse(item[8]), int.Parse(item[9]));
                        else if (item[0] == "Small Potion")
                            inventory[i][j] = new SmallPotion(this);
                        else if (item[0] == "Medium Potion")
                            inventory[i][j] = new MediumPotion(this);
                        else if (item[0] == "Large Potion")
                            inventory[i][j] = new LargePotion(this);
                    }
                }
        }

        public override void mouseUp(object sender, MouseEventArgs e) { }

        public override void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.CloseKey)
            {
                this.addChildState(new PauseState(form), false, true);
            }
            else if (e.KeyCode == Properties.Settings.Default.UpKey)
            {
                hero.dirs[0] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.LeftKey)
            {
                hero.dirs[1] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.DownKey)
            {
                hero.dirs[2] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.RightKey)
            {
                hero.dirs[3] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.InventoryKey)
            {
                this.addChildState(new InventoryState(form), false, false);
            }
            else if (e.KeyCode == Properties.Settings.Default.Attack1Key)
            {
                hero.attacks[0] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.Attack2Key)
            {
                hero.attacks[1] = true;
            }
            else if (e.KeyCode == Properties.Settings.Default.Attack3Key)
            {
                hero.attacks[2] = true;
            }
        }

        public override void keyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Properties.Settings.Default.UpKey)
            {
                hero.dirs[0] = false;
            }
            else if (e.KeyCode == Properties.Settings.Default.LeftKey)
            {
                hero.dirs[1] = false;
            }
            else if (e.KeyCode == Properties.Settings.Default.DownKey)
            {
                hero.dirs[2] = false;
            }
            else if (e.KeyCode == Properties.Settings.Default.RightKey)
            {
                hero.dirs[3] = false;
            }
        }

        public override void mouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                hero.basicAtk();
            }
            else if(e.Button == MouseButtons.Right)
            {
                float x = (float)((e.X - form.ClientSize.Width / 2.0) / size + hero.x);
                float y = (float)((e.Y - form.ClientSize.Height / 2.0) / size + hero.y);
                
                if (Math.Sqrt(Math.Pow(x - hero.x, 2) + Math.Pow(y - hero.y, 2)) < 2)
                {
                    foreach (KeyValuePair<Item, PointF> entry in room.droppedItems)
                        if (Math.Sqrt(Math.Pow(entry.Value.X - x, 2) + Math.Pow(entry.Value.Y - y, 2)) < 1)
                        {
                            if (tryPickupItem(entry.Key))
                                room.droppedItems.Remove(entry.Key);

                            break;
                        }

                    foreach(Obstacle ob in room.obstacles)
                        if (Math.Sqrt(Math.Pow(ob.x - x, 2) + Math.Pow(ob.y - y, 2)) < 1 && ob is Chest)
                        {
                            Chest chest = (Chest)ob;
                            if (chest.closed)
                            {
                                chest.closed = false;
                                room.droppedItems.Add(randomItem(), new PointF(ob.x + 0.5f, ob.y + 0.5f));
                            }
                            break;
                        }

                    if (room.doorSpace[(int) x, (int) y])
                    {
                        Door clickedDoor = new Door(this,-1,-1,0,0,0,true,0,0);
                        foreach (Door door in room.doors)
                        {
                            if ((Math.Sqrt(Math.Pow(door.x - x, 2) + Math.Pow(door.y - y, 2)) < 1)  ||   (Math.Sqrt(Math.Pow((door.x + door.width - 1) - x,2) + Math.Pow((door.y + door.height - 1) - y, 2)) < 1))
                            {
                                // this is the correct door
                                if (!door.switchClosed())
                                {
                                    clickedDoor = door;
                                    break;
                                }
                            }
                        }
                        if (clickedDoor.x != -1)
                        {
                            room.updateDrawingGrid(clickedDoor.getNegativeRoom());
                            room.updateDrawingGrid(clickedDoor.getPositiveRoom());
                        }
                    }
                }
            }
        }

        public override void mouseMove(object sender, MouseEventArgs e)
        {
            hero.dir = (float)Math.Atan2(e.Y - (form.ClientSize.Height / 2), e.X - (form.ClientSize.Width / 2));
        }

        public override void paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(20, 20, 20));

            room.draw(g);
            hero.draw(g);
        }

        public override void tick(object sender, EventArgs e)
        {
            hero.act();

            foreach (Unit unit in room.enemies)
                unit.act();

            foreach (Projectile proj in hero.projectiles)
                proj.act();

            foreach (Unit enemy in room.enemies)
                if (Math.Sqrt(Math.Pow(hero.x - enemy.x, 2) + Math.Pow(hero.y - enemy.y, 2)) < hero.radius + enemy.radius)
                    enemy.attackHero();

            form.Invalidate();
        }
    }
}
