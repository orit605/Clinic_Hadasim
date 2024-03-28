#include <iostream>;
#include <cmath>;

using namespace std;

void option1(int h,int w)
{
	if (abs(h - w) > 5)
		cout << "The area of the rectangular: " << h * w << endl;
	else
		cout << "The perimeter of the rectangular: " << (h + w) * 2 << endl;
}

void option2(int h, int w)
{
	int choise;
	double halfWidth = w / 2.0;
	double halfLength = sqrt(halfWidth * halfWidth + h * h);
	cout << "Press 1 for the perimeter of the triangle and press 2 to print the triangle\n" << endl;
	cin >> choise;
	if (choise == 1)
		cout << "The perimeter of the triangle: " << w + 2 * halfLength<<endl;
	else
	{
		if (w / 2 == 0 | w > h * 2)
			cout << "The triangle can't be printted"<<endl;
		else
		{
			for (int i = 1; i<= h; i++)
			{
				if (i = 1)
				{
					for (int k = 0; k < w / 2; k++)
						cout << " ";
					cout << "*" << endl;
				}
				if (i > 1 && i < h)
				{
					int num_row = h - 2;
					int num_groups = (w - 2) / 2;
					int num_of_rows_in_group = num_row / num_groups;
					int sheerit = num_row % num_groups;
					for (int k = 0; k < num_groups; k++)//קבוצה ראשונה
					{
						if (k = 0)
						{
							for(int m=0;m<num_of_rows_in_group+sheerit;m++)//מספר שורות עם שארית
								for (int n = 0; n < i;n++)
								{

								}
						}
					}
				}
				if (i = h)
				{
					for (int k = 0; k < w; k++)
						cout << "*";
				}
			}
				
		}
	}
}

int main() {
	int choise=0;
	int heightT, widthT;
	//cout << "Press 1 for a rectangular tower.\n Press 2 for a triangle tower.\n Press 3 for exit."<<endl;
	//cin >> choise;
	while (choise != 3) 
	{
		cout << "Press 1 for a rectangular tower.\n Press 2 for a triangle tower.\n Press 3 for exit." << endl;
		cin >> choise;
		if (choise == 1) 
		{
			cout << "You pressed 1. You chose a rectangular tower.\nEnter the height and the width of the tower:" << endl;
			cin >> heightT;
			cin >> widthT;
			option1(heightT, widthT);
		}
		if (choise == 2)
		{
			cout << "You pressed 2. You chose a triangle tower.\nEnter the height and the width of the tower:" << endl;
			cin >> heightT;
			cin >> widthT;
			option2(heightT, widthT);
		}
		else
		{
			cout << "Your input is not correct\n" << endl;
		}
	}
	/*
	switch (choise) {
	case 1:
		
		break;
	case 2:
		
		break;
	case 3:
		cout << "You pressed 3. You chose to exit the program" << endl;
		return 0;
	}
	*/

}