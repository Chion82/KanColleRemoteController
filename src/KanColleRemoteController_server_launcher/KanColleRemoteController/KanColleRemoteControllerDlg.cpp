
// KanColleRemoteControllerDlg.cpp : implementation file
//

#include "stdafx.h"
#include "KanColleRemoteController.h"
#include "KanColleRemoteControllerDlg.h"
#include "afxdialogex.h"
#include <Mshtml.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CString AfxGetDirectory()
{
	char pLocalPath[255];
	memset(pLocalPath, 0, sizeof(pLocalPath));
	GetModuleFileNameA(AfxGetInstanceHandle(), pLocalPath, sizeof(pLocalPath));
	pLocalPath[strrchr(pLocalPath, '\\') - pLocalPath] = '\0';
	CString localPath = CString(pLocalPath);
	return localPath;
}

// CKanColleRemoteControllerDlg dialog

HWND hwnd_IE = NULL;
BOOL CALLBACK CKanColleRemoteControllerDlg::EnumWindowsProc_IE(HWND hwnd, LPARAM lParam)
{
	wchar_t* buf = new wchar_t[30];
	GetClassName(hwnd, buf, 30);
	//if (CString(buf) == L"Internet Explorer_Server")
	if (CString(buf).Find(L"Server")!=-1)
	{
		//:://MessageBox(NULL,L"Child Window Found!",L"",0);
		hwnd_IE = hwnd;
	}
	return TRUE;
}

CKanColleRemoteControllerDlg::CKanColleRemoteControllerDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CKanColleRemoteControllerDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

	//Damo API initalization
	::CoInitialize(NULL);
	damoAPI = new Cdmsoft();
	damoAPI_temp = new Cdmsoft();
	damoAPI_script = new Cdmsoft();
	damoAPI_sortie = new Cdmsoft();
	CLSID clsid;
	HRESULT hr = CLSIDFromProgID(OLESTR("dm.dmsoft"), &clsid);
	damoAPI->CreateDispatch(clsid);
	damoAPI_temp->CreateDispatch(clsid);
	damoAPI_script->CreateDispatch(clsid);
	damoAPI_sortie->CreateDispatch(clsid);

	damoAPI->SetShowErrorMsg(0);
	damoAPI_temp->SetShowErrorMsg(0);
	damoAPI_script->SetShowErrorMsg(0);
	damoAPI_sortie->SetShowErrorMsg(0);
	//Damo API initalization completed

	execCmdLine();

}

CKanColleRemoteControllerDlg::~CKanColleRemoteControllerDlg()
{
	delete damoAPI;
	delete damoAPI_temp;
	delete damoAPI_script;
	delete damoAPI_sortie;
	::CoUninitialize();
}

void CKanColleRemoteControllerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CKanColleRemoteControllerDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON1, &CKanColleRemoteControllerDlg::OnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON2, &CKanColleRemoteControllerDlg::OnBnClickedButton2)
	ON_BN_CLICKED(IDC_BUTTON3, &CKanColleRemoteControllerDlg::OnBnClickedButton3)
	ON_BN_CLICKED(IDC_BUTTON4, &CKanColleRemoteControllerDlg::OnBnClickedButton4)
END_MESSAGE_MAP()


// CKanColleRemoteControllerDlg message handlers

BOOL CKanColleRemoteControllerDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CKanColleRemoteControllerDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CKanColleRemoteControllerDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CKanColleRemoteControllerDlg::execCmdLine()
{
	CString cmdLineStr = GetCommandLine();
	if (cmdLineStr.Find(L"rungame") != -1)
	{
		gameInit();
		exit(0);
	}
	else if (cmdLineStr.Find(L"runscript") != -1)
	{
		scriptInit();
		exit(0);
	}
	else if (cmdLineStr.Find(L"quitgame") != -1)
	{
		gameExit();
		exit(0);
	}
	else if (cmdLineStr.Find(L"quitscript") != -1)
	{
		scriptExit();
		exit(0);
	}
	else if (cmdLineStr.Find(L"runall") != -1)
	{
		gameInit();
		Sleep(30 * 1000);
		scriptInit();
		Sleep(5 * 1000);
		gameWatchDog();
		exit(0);
	}
	else if (cmdLineStr.Find(L"quitall") != -1)
	{
		scriptExit();
		gameExit();
		exit(0);
	}
	else if (cmdLineStr.Find(L"capture") != -1)
	{
		capture();
		exit(0);
	}
}

void CKanColleRemoteControllerDlg::capture()
{
	HWND targetParent = ::FindWindow(NULL, L"提督業も忙しい！");
	if (targetParent == NULL)
	{
		//MessageBox(L"Parent window Not Found!");
		return;
	}
	bindEmbededIE();
	damoAPI->Capture(0, 0, 2000, 2000, L"capture.bmp");
}

void CKanColleRemoteControllerDlg::bindEmbededIE()
{
	//Found Embed IE window hwnd
	long bindResult = 0;
	while (bindResult == 0)
	{
		HWND targetParent = NULL;
		HWND targetHwnd = NULL;
		do {
			targetParent = ::FindWindow(NULL, L"提督業も忙しい！");
			Sleep(1000);
		} while (targetParent == NULL);
		if (targetParent == NULL)
		{
			//MessageBox(L"Parent window Not Found!");
		}
		else {
			::ShowWindow(targetParent, SW_NORMAL);
			::SetForegroundWindow(targetParent);
			EnumChildWindows(targetParent, CKanColleRemoteControllerDlg::EnumWindowsProc_IE, NULL);
		}
		targetHwnd = hwnd_IE;
		bindResult = damoAPI->BindWindow((long)targetHwnd, L"gdi", L"windows", L"windows", 0);
		Sleep(1000);
	}
	//damoAPI->Capture(0, 0, 2000, 2000, L"test.bmp");
}

void CKanColleRemoteControllerDlg::clickWebStartButton()
{
	//findAndClickButton(L"web_start.bmp", damoAPI);
	int findResult = -1;
	while (findResult == -1) {
		findResult = GetHTMLSourceFromHwnd(hwnd_IE).Find(L"runGameStart");
		Sleep(2000);
	}
	//Sleep(5000);
	clickCoordinate(767, 242, damoAPI);
}

void CKanColleRemoteControllerDlg::clickWebLoginButton()
{
	//findAndClickButton(L"web_login_google.bmp", damoAPI);
	int findResult = -1;
	while (findResult == -1) {
		findResult = GetHTMLSourceFromHwnd(hwnd_IE).Find(L"btn btn-sns-google");
		Sleep(2000);
	}
	//Sleep(5000);
	//clickCoordinate(640, 328, damoAPI);
	findAndClickButton(L"web_login_google.bmp", damoAPI);
}

void CKanColleRemoteControllerDlg::clickGameStartButton()
{
	findAndClickButton(L"game_start.bmp", damoAPI);
}

void CKanColleRemoteControllerDlg::gameWatchDog()
{
	int count = 0;
	VARIANT resultX, resultY;
	while (true) {
		Sleep(1000);
		count++;
		if (-1 != damoAPI->FindPic(0, 0, 2000, 2000, L"game_start_sortie.bmp", L"000000", 0.7, 0, &resultX, &resultY))
		{
			count = 0;
		}
		if (count > 60 * 15)
		{
			clickCoordinate(20, 20, damoAPI);
		}
	}
}

void CKanColleRemoteControllerDlg::gameInit()
{
	WinExec("KanColleViewer\\KanColleViewer.exe", SW_SHOWNORMAL);
	Sleep(5000);
	bindEmbededIE();
	Sleep(1000);
	clickWebStartButton();
	Sleep(1000);
	clickWebLoginButton();
	Sleep(1000);
	clickGameStartButton();
	Sleep(1000);
}

void CKanColleRemoteControllerDlg::scriptInit()
{
	//WinExec("AutoScript\\autoscript.exe", SW_SHOWNORMAL);
	//ShellExecute(NULL, L"open", L"AutoScript\\autoscript.exe", L"", L"", SW_NORMAL);
	WinExec("runscript.bat", SW_SHOWNORMAL);
	Sleep(5000);
	bindScript();
	Sleep(5000);
	clickAutoGetHandleButton();
	Sleep(1000);
	selectExpeditionFleet();
	Sleep(1000);
	sortie();
	Sleep(1000);
}

void CKanColleRemoteControllerDlg::gameExit()
{
	WinExec("taskkill /F /IM KanColleViewer.exe",SW_SHOWNORMAL);
}

void CKanColleRemoteControllerDlg::scriptExit()
{
	WinExec("taskkill /F /IM autoscript.exe", SW_SHOWNORMAL);
}

void CKanColleRemoteControllerDlg::bindScript()
{
	HWND targetScriptHwnd = NULL;
	long bindResult = 0;
	while (bindResult==0)
	{
		do {
			targetScriptHwnd = ::FindWindow(NULL, L"奏的舰队脚本(新版)  v3.3.3.3");
			Sleep(1000);
		} while (targetScriptHwnd == NULL);
	
		bindResult = damoAPI_script->BindWindow((long)targetScriptHwnd, L"gdi", L"windows3", L"windows", 0);
		Sleep(1000);
	}
	//damoAPI_script->Capture(0, 0, 2000, 2000, L"test.bmp");
}

void CKanColleRemoteControllerDlg::clickAutoGetHandleButton()
{
	clickCoordinate(284, 198, damoAPI_script);
}

void CKanColleRemoteControllerDlg::selectExpeditionFleet()
{
	char buf[10];
	FILE* confFile = fopen("expedition.conf", "r");
	int charCount = fread(buf, 1, 10, confFile);
	fclose(confFile);
	buf[charCount] = '\0';
	CString confStr = CString(buf);
	////MessageBox(confStr);
	if (confStr.Find(CString("2")) == -1)
	{
		clickCoordinate(21, 50, damoAPI_script);
		Sleep(1000);
	}
	if (confStr.Find(CString("3")) == -1)
	{
		clickCoordinate(24, 95, damoAPI_script);
		Sleep(1000);
	}
	if (confStr.Find(CString("4")) == -1)
	{
		clickCoordinate(23, 134, damoAPI_script);
		Sleep(1000);
	}
	Sleep(1000);
	clickCoordinate(391, 198, damoAPI_script);
}

void CKanColleRemoteControllerDlg::sortie()
{
	char buf[10];
	FILE* confFile = fopen("sortie.conf", "r");
	int charCount = fread(buf, 1, 10, confFile);
	fclose(confFile);
	buf[charCount] = '\0';
	CString confStr = CString(buf);
	if (confStr != "1")
		return;
	clickCoordinate(133, 242, damoAPI_script);
	Sleep(1000);
	clickCoordinate(307, 205, damoAPI_script);
	Sleep(3000);
	//damoAPI_temp->KeyPress(VK_RETURN);
	Cdmsoft temp;
	CLSID clsid;
	HRESULT hr = CLSIDFromProgID(OLESTR("dm.dmsoft"), &clsid);
	temp.CreateDispatch(clsid);
	temp.SetShowErrorMsg(0);

	long bindResult = 0;
	while (bindResult == 0)
	{
		HWND tempHwnd = ::FindWindow(NULL, L"使用须知");
		bindResult = temp.BindWindow((long)tempHwnd, L"gdi", L"windows3", L"windows", 0);
		Sleep(1000);
	}
	temp.KeyPress(VK_RETURN);

	Sleep(1000);
	bindSortieWindow();
	Sleep(1000);
	loadSortieConf();
	Sleep(1000);
	startSortie();
}

void CKanColleRemoteControllerDlg::bindSortieWindow()
{
	long bindResult = 0;
	while (bindResult == 0)
	{
		HWND targetWindow = ::FindWindow(NULL, L"[开发&测试中的功能] 出击设置  v3.3.3");
		if (targetWindow == NULL)
			return;
		bindResult = damoAPI_sortie->BindWindow((long)targetWindow, L"gdi", L"windows3", L"windows", 0);
		Sleep(1000);
	}
}

void CKanColleRemoteControllerDlg::loadSortieConf()
{
	clickCoordinate(669, 398, damoAPI_sortie);
	Sleep(8000);
	clickCoordinate(267, 31, damoAPI_sortie);
	Sleep(8000);
	CString confFilePath = AfxGetDirectory() + L"\\AutoScript\\sortieconfig.ini";

	HWND tempHwnd = NULL;
	while (tempHwnd == NULL)
	{
		tempHwnd = ::FindWindow(NULL, L"打开配置文件");
		Sleep(1000);
	}
	HWND targetCombo = ::FindWindowEx(tempHwnd, NULL, L"ComboBoxEx32", NULL);
	Sleep(5000);
	::SendMessage(targetCombo, WM_SETTEXT, NULL, (LPARAM)confFilePath.GetBuffer());
	Sleep(5000);
	HWND targetButton = ::FindWindowEx(tempHwnd, NULL, L"Button", NULL);
	::SendMessage(targetButton, WM_LBUTTONDOWN, 0, 0);
	::SendMessage(targetButton, WM_LBUTTONUP, 0, 0);
	
}

void CKanColleRemoteControllerDlg::startSortie()
{
	//clickCoordinate(61, 493, damoAPI_sortie);
	Sleep(1000);
	clickCoordinate(642, 470, damoAPI_sortie);
	Sleep(1000);
	clickCoordinate(212, 526, damoAPI_sortie);
}

void CKanColleRemoteControllerDlg::findAndClickButton(CString buttonBmp, Cdmsoft* damoAPI)
{
	VARIANT resultX, resultY;
	long findResult = -1;
	while (findResult == -1)
	{
		HWND scriptErrorHwnd = ::FindWindow(NULL, L"Internet Explorer_TridentDlgFrame");
		if (scriptErrorHwnd != NULL) {
			damoAPI_temp->BindWindow((long)scriptErrorHwnd, L"gdi", L"windows", L"windows", 0);
			damoAPI_temp->KeyPress(VK_RETURN);
			Sleep(500);
		}
		findResult = damoAPI->FindPic(0, 0, 2000, 2000, buttonBmp, L"000000", 0.7, 0, &resultX, &resultY);
		Sleep(500);
	}
	//MessageBox(L"Button Found!");
	damoAPI->MoveTo(resultX.intVal, resultY.intVal);
	damoAPI->LeftClick();
}

void CKanColleRemoteControllerDlg::clickCoordinate(int x, int y, Cdmsoft* damoAPI)
{
	HWND scriptErrorHwnd = ::FindWindow(NULL, L"Internet Explorer_TridentDlgFrame");
	if (scriptErrorHwnd != NULL) {
		damoAPI_temp->BindWindow((long)scriptErrorHwnd, L"gdi", L"windows", L"windows", 0);
		damoAPI_temp->KeyPress(VK_RETURN);
		Sleep(500);
	}
	damoAPI->MoveTo(x, y);
	damoAPI->LeftClick();
}

BOOL CALLBACK CKanColleRemoteControllerDlg::enumFindAndClickButton(HWND hwnd, LPARAM buttonCaption)
{
	wchar_t buf[50];
	::SendMessage(hwnd, WM_GETTEXT, 50, (LPARAM)buf);
	if (CString(buf) == CString((wchar_t*)buttonCaption))
	{
		::SendMessage(hwnd, WM_LBUTTONDOWN, NULL, NULL);
		::SendMessage(hwnd, WM_LBUTTONUP, NULL, NULL);
	}
	return TRUE;
}




void CKanColleRemoteControllerDlg::OnBnClickedButton3()
{
	gameExit();
}

void CKanColleRemoteControllerDlg::OnBnClickedButton4()
{
	scriptExit();
}

void CKanColleRemoteControllerDlg::OnClickedButton1()
{
	gameInit();
}

void CKanColleRemoteControllerDlg::OnBnClickedButton2()
{
	scriptInit();
}

CString CKanColleRemoteControllerDlg::GetHTMLSourceFromHwnd(HWND BrowserWnd)
{
	//CoInitialize(NULL);
	HRESULT hr;
	// Explicitly load MSAA so we know if it's installed
	HINSTANCE hInst = ::LoadLibrary(_T("OLEACC.DLL"));
	if (hInst)
	{
		LRESULT lRes; //SendMessageTimeout后的返回值，用于函数pfObjectFromLresult的第1个参数
		UINT nMsg = ::RegisterWindowMessage(_T("WM_HTML_GETOBJECT"));
		::SendMessageTimeout(BrowserWnd, nMsg, 0L, 0L, SMTO_ABORTIFHUNG, 10000, (DWORD*)&lRes);
		//获取函数pfObjectFromLresult
		LPFNOBJECTFROMLRESULT pfObjectFromLresult = (LPFNOBJECTFROMLRESULT)::GetProcAddress(hInst, ("ObjectFromLresult"));
		if (pfObjectFromLresult)
		{
			CComPtr<IHTMLDocument2> spDoc;
			hr = (*pfObjectFromLresult)(lRes, IID_IHTMLDocument, 0, (void**)&spDoc);
			if (SUCCEEDED(hr))
			{
				CString strReturn("");
				CComPtr<IHTMLDocument3> pDocument3;
				spDoc->QueryInterface(IID_IHTMLDocument3, (void**)&pDocument3);
				CComPtr<IHTMLElement> tagElement;
				pDocument3->get_documentElement(&tagElement);
				CComBSTR bstr;
				tagElement->get_outerHTML(&bstr);
				strReturn = OLE2T(bstr);
				return strReturn;
			} // else document not ready
		} // else Internet Explorer is not running
		::FreeLibrary(hInst);
	} // else Active Accessibility is not installed
	//CoUninitialize();
	return CString("");
}