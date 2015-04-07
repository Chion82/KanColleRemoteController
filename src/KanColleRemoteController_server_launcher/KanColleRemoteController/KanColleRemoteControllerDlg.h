
// KanColleRemoteControllerDlg.h : header file
//

#pragma once

#include "Cdmsoft.h"


// CKanColleRemoteControllerDlg dialog
class CKanColleRemoteControllerDlg : public CDialogEx
{
public:
	// Construction
	CKanColleRemoteControllerDlg(CWnd* pParent = NULL);	// standard constructor
	~CKanColleRemoteControllerDlg();

	Cdmsoft* damoAPI = NULL;
	Cdmsoft* damoAPI_temp = NULL;
	Cdmsoft* damoAPI_script = NULL;
	Cdmsoft* damoAPI_sortie = NULL;

	static BOOL CALLBACK EnumWindowsProc_IE(_In_  HWND hwnd, _In_  LPARAM lParam);
	static BOOL CALLBACK enumFindAndClickButton(HWND hwnd, LPARAM buttonCaption);

	void gameInit();
	void gameExit();
	
	void scriptInit();
	void scriptExit();

	void capture();

	void execCmdLine();

protected:
	void bindEmbededIE();
	void clickWebStartButton();
	void clickWebLoginButton();
	void clickGameStartButton();
	void gameWatchDog();

	void bindScript();
	void clickAutoGetHandleButton();
	void selectExpeditionFleet();

	void sortie();
	void bindSortieWindow();
	void loadSortieConf();
	void startSortie();


	CString GetHTMLSourceFromHwnd(HWND BrowserWnd);
	void findAndClickButton(CString buttonBmp, Cdmsoft* damoAPI);
	void clickCoordinate(int x, int y, Cdmsoft* damoAPI);

// Dialog Data
	enum { IDD = IDD_KANCOLLEREMOTECONTROLLER_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnClickedButton1();
	afx_msg void OnBnClickedButton2();
	afx_msg void OnBnClickedButton3();
	afx_msg void OnBnClickedButton4();
};
