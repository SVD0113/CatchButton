using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatchButton
{
    public partial class Form1 : Form
    {
        // <제미나이> Win32 Api활용
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        // 필드 선언
        private Random rd;
        private int x;
        private double randomDouble;

        // 점수 변수
         private int score = 0;

        // 놓친 점수 기억
        private int misscount = 0;

        // 우클릭 횟수 
        private int RightClickCount = 0;

        public Form1()
        {
            InitializeComponent();

            // 초기화 시점에 Random 객체를 생성하고 난수를 생성합니다.
            rd = new Random(); // 랜덤객체생성
            x = rd.Next(0, 500);  // 0 이상500 미만의정수반환
            randomDouble = rd.NextDouble(); // 0.0과1.0 사이의실수반환
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void run_MouseEnter(object sender, EventArgs e)
        {

            // 1. 20번 놓치면 게임 오버
            misscount++;

            if (misscount >= 20)
            {
                MessageBox.Show("20번 놓쳐서 Game Over!");

                run.Enabled = false; // 모든 버튼 비활성화

                return;
            }

            // 2. 난수 생성기 준비
            Random localRd = new Random();

            // 3. 가용 영역 계산(버튼이 폼 테두리에 걸리지 않게 보호)
            int maxX = this.ClientSize.Width - run.Width;
            int maxY = this.ClientSize.Height - run.Height;
            if (maxX < 0) maxX = 650;
            if (maxY < 0) maxY = 400;

            // 4. 랜덤 좌표 추출(0 ~ 최대 가용치 사이)
            int nextX = localRd.Next(0, maxX + 1);
            int nextY = localRd.Next(0, maxY + 1);

            // 5．마우스　포인터　변경
            IntPtr ptrHover = LoadCursorFromFile("pointer.ani");
            if (ptrHover != IntPtr.Zero) run.Cursor = new Cursor(ptrHover);

            // 6. 위치 할당(새로운 Point 객체 생성)
            run.Location = new Point(nextX, nextY);

            // 7. 놓쳤으니 점수 마이너스
            score -= 3;

            // 8. 폼 제목 표시줄에 좌표 출력
            this.Text = $"현재 점수: {score}점 | 놓친 횟수: {misscount}번 | 버튼위치: ({nextX}, {nextY})"; 

            // 9. 놓쳤을 때 효과음
            System.Media.SoundPlayer escapeSound = new System.Media.SoundPlayer("jingles_PIZZI07.wav");
            escapeSound.Play();
            
        }

        private void run_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void run_MouseDown(object sender, MouseEventArgs e)
        {
            // 1. 좌클릭
            if (e.Button == MouseButtons.Left)
            {
                // 좌클릭 포인터 변경 
                IntPtr ptrLeft = LoadCursorFromFile("alternate.ani");
                if (ptrLeft != IntPtr.Zero) this.Cursor = new Cursor(ptrLeft);

                // 잡았으니 점수 플러스
                score += 20;

                // 점수 즉시 반영
                this.Text = $"현재 점수: {score}점 | 놓친 횟수: {misscount}번";

                // 성공 효과음
                System.Media.SoundPlayer catchSound = new System.Media.SoundPlayer("jingles_STEEL06.wav");
                catchSound.Play();

                // 포인터 보여주는 시간
                Application.DoEvents();
                System.Threading.Thread.Sleep(1000);

                //　좌클릭으로　잡았을　때
                MessageBox.Show("성공!, +20점, 놓친 횟수 -1");

                // 좌클릭은 버튼 크기 1%씩 축소 (*0.9)
                run.Width = (int)(run.Width * 0.9);
                run.Height = (int)(run.Height * 0.9);

                // 잡으면 놓친 횟수 -1
                misscount -= 1;

                // 포인터 복구 (up안됨)
                this.Cursor = Cursors.Default;
            }

            // 2. 우클릭
            else if (e.Button == MouseButtons.Right)
            {
                // 우클릭 클릭 제한
                if (RightClickCount < 5)
                {
                    RightClickCount++;

                    // 우클릭 포인터 변경 
                    IntPtr ptrRight = LoadCursorFromFile("help.ani");
                    if (ptrRight != IntPtr.Zero) this.Cursor = new Cursor(ptrRight);

                    // 우클릭으로 잡았으니 점수 조금만 플러스
                    score += 10;

                    // 우클릭은 버튼 크기 2%씩 확대 (*1.2)
                    run.Width = (int)(run.Width * 1.2);
                    run.Height = (int)(run.Height * 1.2);

                    // 잡으면 놓친 횟수 -1
                    misscount -= 1;

                    // 포인터 보여주는 시간
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);

                    // 우클릭으로 잡았을 때
                    MessageBox.Show("좌클릭으로 다시!");

                    // 잡으면 놓친 횟수 -3
                    misscount -= 3;

                    // 점수 즉시 반영
                    this.Text = $"현재 점수: {score} | 놓친 횟수: {misscount}";

                    // 포인터 복구 (up안됨)
                    this.Cursor = Cursors.Default;
                }

                else
                {
                    // 우클릭 제한 후에도 enter처럼 진행
                    run_MouseEnter(sender, e);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. 점수, 놓친 횟수 초기화
            score = 0;
            misscount = 0;
            RightClickCount = 0;

            // 2. 폼 제목도 초기화
            this.Text = "New Game Start";

            // 3. 버튼 크기 초기화
            run.Width = 170;
            run.Height = 100;

            // 4. CatchButton 활성화
            run.Enabled = true;
        }
    }
}
