module.exports = {
  'GET /sitecore/api/ssc/sci/translate/12990C27-0517-4EBD-A5A1-D0DE1BA41997/gettranslations': function (req, res) {
    res.json([
      { Phrase: 'Back', Key: 'BACK' },
      { Phrase: 'xConnectBI', Key: 'APP_NAME' },
      { Phrase: 'Business Intelligence Dashboard', Key: 'DASHBOARD_PAGE' },
      { Phrase: 'Dashboard', Key: 'NAV_CATEGORY1' },
      { Phrase: 'Sales', Key: 'MENU1' },
      { Phrase: 'Yearly', Key: 'MENU2' },
      { Phrase: 'Log out', Key: 'LOG_OUT' }      
    ]);
  }
};
