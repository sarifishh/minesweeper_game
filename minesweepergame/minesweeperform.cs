using System.Data.Common;

namespace minesweepergame

{
    public partial class minesweeperform : Form
    {
        private Button[,] gridButtons;
        private const int SQUARE_GRID_SIZE = 20;
        private const int cols = SQUARE_GRID_SIZE;
        
        private const int gridWidth = 1200;
        private const int gridHeight = 800;
        private const int tileWidth = gridWidth / cols;
        private const int rows = gridHeight/tileWidth;
        private const int tileHeight = tileWidth;
        private const int gridTop = 100;
        private const int gridLeft = 50;
        private const int numMines = 70;
        private int[,] mines = new int[rows,cols];
        bool firstclick = true;
        Random random = new Random();

        public minesweeperform()
        {
            InitializeComponent();
            CreateGrid(rows, cols, tileWidth, tileHeight, gridTop, gridLeft);
            NewGame();
        }

        private void NewGame()
        {
            firstclick = true;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    gridButtons[r, c].Text = "";
                    gridButtons[r, c].Enabled = true;
                    gridButtons[r, c].BackColor = Color.Gray;
                }
            }
            
        }

        private void CreateGrid(int rows, int cols, int tileWidth, int tileHeight, int gridTop, int gridLeft)
        {
            gridButtons = new Button[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    gridButtons[r, c] = new Button()
                    {
                        Size = new Size(tileWidth, tileHeight),
                        Location = new Point(tileWidth * c + gridLeft, tileHeight * r + gridTop),
                        BackColor = Color.Beige,
                        Font = new Font("Arial", gridWidth / cols / 3),
                        Name = Convert.ToString(r) + " " + Convert.ToString(c),
                    };
                    gridButtons[r, c].ForeColor = Color.Black;
                    gridButtons[r, c].MouseDown += new MouseEventHandler(GridButtonClick);
                    Controls.Add(gridButtons[r, c]);
                }
            }
        }

        private void GenerateMineMap(int clickedr, int clickedc)
        {
            mines[clickedr, clickedc] = 2; //making where the user clicked and the spaces around that not 0 so mines arent placed there
            if (clickedr > 0)
            {
                if(clickedc > 0)
                {
                    mines[clickedr-1, clickedc-1] = 2;
                    
                }
                mines[clickedr-1, clickedc] = 2;
                if (clickedc < cols-1)
                {
                    mines[clickedr - 1, clickedc + 1] = 2;
                }
            }
            if (clickedr < rows-1)
            {
                if (clickedc > 0)
                {
                    mines[clickedr + 1, clickedc - 1] = 2;
                }
                mines[clickedr + 1, clickedc] = 2;
                if (clickedc < cols - 1)
                {
                    mines[clickedr + 1, clickedc + 1] = 2;
                    
                }
            }
            if (clickedc < cols - 1)
            {
                mines[clickedr, clickedc + 1] = 2;

            }
            if (clickedc > 0)
            {
                mines[clickedr, clickedc - 1] = 2;

            }

            for (var r = 0; r < numMines; ++r)
            {
                int colnum = random.Next(0, cols-1);
                int rownum = random.Next(0, rows-1);

                if (mines[rownum,colnum] == 0)
                {
                    mines[rownum,colnum] = 1;
                }
                else
                {
                    r--; //if theres already a mine in the randomly selected coordinate it will make the loop run an extra time
                }
            }
           
        }

        
        private void GridButtonClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                string[] coordinates = (((Button)sender).Name).Split(' '); //getting the corrdinates from the string name so the box position can be compared with the mine map array
                int row = Convert.ToInt32( coordinates[0]);
                int col = Convert.ToInt32(coordinates[1]);
                if (firstclick)
                {
                    GenerateMineMap(row, col);
                    firstclick = false;
                }
                if (mines[row,col] == 1)
                {
                    DisplayMines();
                    DisableGrid();
                    
                    MessageBox.Show("Mine Hit. Game Over");
                }
                else
                {
                    DisableMine(row, col);
                    CheckIfWon();
                }


            }
            if (e.Button == MouseButtons.Right)
            {
               
                if (((Button)sender).Text == "")
                {
                    ((Button)sender).Text = "F";
                }
                else if (((Button)sender).Text == "F")
                {
                    ((Button)sender).Text = "";

                }
            }
        }
        private void DisplayMines()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if ((mines[r,c] == 1))
                    {
                        gridButtons[r, c].Text = "M";
                    }
                    
                }
            }
        }

        private void CheckIfWon()
        {
            bool won = true;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if ((mines[r, c] == 0))
                    {
                        won = false;
                        break;
                    }

                }
            }
            if (won)
            {
                DisableGrid();
                MessageBox.Show("Only mines remain - game won!");
            }
        }

        private void CountSurroundingMines(int r, int c)
        {
            if (r > 0)
            {
                if (c > 0)
                {
                    EditSurroundingMineNum(r - 1, c - 1);
                   

                }
                EditSurroundingMineNum(r - 1, c );
                if (c < cols - 1)
                {
                    EditSurroundingMineNum(r - 1, c + 1);
                }
            }
            if (r < rows - 1)
            {
                if (c > 0)
                {
                    EditSurroundingMineNum(r + 1, c - 1);
                }
                EditSurroundingMineNum(r + 1, c );
                if (c < cols - 1)
                {
                    EditSurroundingMineNum(r + 1, c + 1);

                }
            }
            if (c < cols - 1)
            {
                EditSurroundingMineNum(r , c + 1);

            }
            if (c > 0)
            {
                EditSurroundingMineNum(r, c - 1);

            }
        }


        private void EditSurroundingMineNum(int r, int c)
        {
            if (gridButtons[r, c].Enabled == true)
            {
                int surroundingmines = 0;
                if (r > 0)
                {
                    if (c > 0)
                    {
                        if (mines[r - 1, c - 1] == 1)
                        {
                            surroundingmines++;
                        }

                    }
                    if (mines[r - 1, c] == 1)
                    {
                        surroundingmines++;
                    }
                    if (c < cols - 1)
                    {
                        if (mines[r - 1, c + 1] == 1)
                        {
                            surroundingmines++;
                        }
                    }
                }
                if (r < rows - 1)
                {
                    if (c > 0)
                    {
                        if (mines[r + 1, c - 1] == 1)
                        {
                            surroundingmines++;
                        }
                    }
                    if (mines[r + 1, c] == 1)
                    {
                        surroundingmines++;
                    }
                    if (c < cols - 1)
                    {
                        if (mines[r + 1, c + 1] == 1)
                        {
                            surroundingmines++;
                        }

                    }
                }
                if (c < cols - 1)
                {
                    if (mines[r, c + 1] == 1)
                    {
                        surroundingmines++;
                    }

                }
                if (c > 0)
                {
                    if (mines[r, c - 1] == 1)
                    {
                        surroundingmines++;
                    }

                }
                if (surroundingmines == 0)
                {
                    DisableMine(r, c);
                }
                else
                {
                    gridButtons[r, c].Text = Convert.ToString(surroundingmines);
                }
            }
        }
        
        private void DisableMine(int row, int column)
        {
            gridButtons[row, column].Enabled = false;
            gridButtons[row, column].BackColor = Color.LightGray;
            gridButtons[row, column].Text = string.Empty;
            CountSurroundingMines(row, column);
            mines[row, column] = 3;
        }
        private void DisableGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    gridButtons[r, c].Enabled = false;
                }
            }
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateGrid(rows, cols, tileWidth, tileHeight, gridTop, gridLeft);
            NewGame();
        }
    }
}