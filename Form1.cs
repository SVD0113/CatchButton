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

            // 1. 난수 생성기 준비
            Random localRd = new Random();

            // 2. 가용 영역 계산(버튼이 폼 테두리에 걸리지 않게 보호)
            // ClientSize는 타이틀바와 테두리를 제외한 실제 그리기 영역입니다.
            int maxX = this.ClientSize.Width - run.Width;
            int maxY = this.ClientSize.Height - run.Height;
            if (maxX < 0) maxX = 650;
            if (maxY < 0) maxY = 400;

            // 3. 랜덤 좌표 추출(0 ~ 최대 가용치 사이)
            int nextX = localRd.Next(0, maxX + 1);
            int nextY = localRd.Next(0, maxY + 1);

            // ４．마우스　포인터　변경 (제미나이 도움)
            IntPtr ptrHover = LoadCursorFromFile("pointer.ani");
            if (ptrHover != IntPtr.Zero) run.Cursor = new Cursor(ptrHover);

            // ５. 위치 할당(새로운 Point 객체 생성)
            run.Location = new Point(nextX, nextY);

            // ６. 시각적 피드백(폼 제목 표시줄에 좌표 출력)
            this.Text = $"버튼위치: ({nextX}, {nextY})";

            // ７. 놓쳤을 때 효과음
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
                // 좌클릭 포인터 변경 (제미나이)
                IntPtr ptrLeft = LoadCursorFromFile("alternate.ani");
                if (ptrLeft != IntPtr.Zero) this.Cursor = new Cursor(ptrLeft);

                // 성공 효과음
                System.Media.SoundPlayer catchSound = new System.Media.SoundPlayer("jingles_STEEL06.wav");
                catchSound.Play();

                // 포인터 보여주는 시간
                Application.DoEvents();
                System.Threading.Thread.Sleep(1000);

                //　좌클릭으로　잡았을　때
                MessageBox.Show("축하합니다~!", "성공");

                // 포인터 복구 (up안됨)
                this.Cursor = Cursors.Default;
            }

            // 2. 우클릭
            else if (e.Button == MouseButtons.Right)
            {
                // 우클릭 포인터 변경 (제미나이)
                IntPtr ptrRight = LoadCursorFromFile("help.ani");
                if (ptrRight != IntPtr.Zero) this.Cursor = new Cursor(ptrRight);

                // 포인터 보여주는 시간
                Application.DoEvents();
                System.Threading.Thread.Sleep(1000);

                // 우클릭으로 잡았을 때
                MessageBox.Show("좌클릭으로 잡으세요", "다시");

                // 포인터 복구 (up안됨)
                this.Cursor = Cursors.Default;
            }
        }
    }
}
