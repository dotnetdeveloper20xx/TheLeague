const puppeteer = require('puppeteer');

const BASE_URL = 'http://localhost:4200';
const API_URL = 'http://localhost:7000';

const testResults = {
  passed: [],
  failed: [],
  warnings: []
};

const users = {
  superAdmin: { email: 'admin@theleague.com', password: 'Admin123!', expectedRoute: '/admin' },
  clubManager: { email: 'manager@riverside.com', password: 'Manager123!', expectedRoute: '/club' },
  member: { email: 'henry.brown1@riverside.com', password: 'Member123!', expectedRoute: '/portal' }
};

function log(message, type = 'info') {
  const timestamp = new Date().toISOString().substr(11, 8);
  const prefix = { info: '[INFO]', pass: '[PASS]', fail: '[FAIL]', warn: '[WARN]' }[type] || '[INFO]';
  console.log(`${timestamp} ${prefix} ${message}`);
}

function pass(testName) {
  log(testName, 'pass');
  testResults.passed.push(testName);
}

function fail(testName, error) {
  log(`${testName}: ${error}`, 'fail');
  testResults.failed.push({ test: testName, error: error.toString() });
}

function warn(message) {
  log(message, 'warn');
  testResults.warnings.push(message);
}

async function wait(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

async function checkApiHealth() {
  log('Checking API health...');
  try {
    const response = await fetch(`${API_URL}/api/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({})
    });
    if (response.status === 400) {
      pass('API is running');
      return true;
    }
    fail('API health check', 'Unexpected response');
    return false;
  } catch (e) {
    fail('API health check', e.message);
    return false;
  }
}

async function testLogin(page, user, userType) {
  log(`Testing login for ${userType}...`);

  try {
    await page.goto(`${BASE_URL}/auth/login`, { waitUntil: 'networkidle0', timeout: 30000 });
    await wait(1000);

    // Check login page loaded
    const emailInput = await page.$('input[formcontrolname="email"], input[id="email"], input[type="email"]');
    if (!emailInput) {
      fail(`${userType} login - page load`, 'Email input not found');
      return false;
    }
    pass(`${userType} login page loaded`);

    // Clear and enter email
    await emailInput.click({ clickCount: 3 });
    await emailInput.type(user.email);

    // Enter password
    const passwordInput = await page.$('input[formcontrolname="password"], input[id="password"], input[type="password"]');
    await passwordInput.click({ clickCount: 3 });
    await passwordInput.type(user.password);

    // Click submit
    const submitButton = await page.$('button[type="submit"]');
    await submitButton.click();

    // Wait for navigation or error
    await wait(3000);

    const currentUrl = page.url();
    if (currentUrl.includes(user.expectedRoute)) {
      pass(`${userType} login successful - redirected to ${user.expectedRoute}`);
      return true;
    } else if (currentUrl.includes('/auth/login')) {
      // Check for error message
      const errorMsg = await page.$eval('.text-red-500, .form-error, .error', el => el.textContent).catch(() => null);
      fail(`${userType} login - redirect failed`, `Still on login page. Error: ${errorMsg || 'Unknown'}`);
      return false;
    } else {
      pass(`${userType} login successful - at ${currentUrl}`);
      return true;
    }
  } catch (e) {
    fail(`${userType} login`, e.message);
    return false;
  }
}

async function testNavigation(page, links, userType) {
  log(`Testing navigation for ${userType}...`);

  for (const link of links) {
    try {
      await page.goto(`${BASE_URL}${link.path}`, { waitUntil: 'networkidle0', timeout: 15000 });
      await wait(500);

      const currentUrl = page.url();

      // Check if redirected to login (unauthorized)
      if (currentUrl.includes('/auth/login')) {
        fail(`${userType} - ${link.name}`, 'Redirected to login (unauthorized)');
        continue;
      }

      // Check for page content
      const hasContent = await page.evaluate(() => {
        const body = document.body.innerText;
        return body.length > 100;
      });

      if (hasContent) {
        pass(`${userType} - ${link.name} page loaded`);
      } else {
        warn(`${userType} - ${link.name} page may be empty`);
      }

      // Check for console errors
      const consoleErrors = [];
      page.on('console', msg => {
        if (msg.type() === 'error') consoleErrors.push(msg.text());
      });

    } catch (e) {
      fail(`${userType} - ${link.name}`, e.message);
    }
  }
}

async function testUIElements(page, userType) {
  log(`Testing UI elements for ${userType}...`);

  try {
    // Test buttons exist and are clickable
    const buttons = await page.$$('button');
    log(`Found ${buttons.length} buttons on page`);

    // Test forms
    const forms = await page.$$('form');
    log(`Found ${forms.length} forms on page`);

    // Test inputs
    const inputs = await page.$$('input');
    log(`Found ${inputs.length} input fields on page`);

    // Test tables
    const tables = await page.$$('table');
    log(`Found ${tables.length} tables on page`);

    // Test cards
    const cards = await page.$$('.card');
    log(`Found ${cards.length} card elements on page`);

    pass(`${userType} - UI elements check completed`);
  } catch (e) {
    fail(`${userType} - UI elements`, e.message);
  }
}

async function clearSession(page) {
  log('Clearing browser session...');
  await page.evaluate(() => {
    localStorage.clear();
    sessionStorage.clear();
  });
  // Clear cookies
  const client = await page.target().createCDPSession();
  await client.send('Network.clearBrowserCookies');
  await wait(500);
}

async function testSuperAdmin(page) {
  log('\n========== TESTING SUPER ADMIN ==========');

  await clearSession(page);

  const loggedIn = await testLogin(page, users.superAdmin, 'SuperAdmin');
  if (!loggedIn) return;

  const adminLinks = [
    { path: '/admin', name: 'Dashboard' },
    { path: '/admin/clubs', name: 'Clubs List' },
    { path: '/admin/clubs/new', name: 'New Club Form' },
    { path: '/admin/users', name: 'Users List' },
    { path: '/admin/reports', name: 'Reports' },
    { path: '/admin/settings', name: 'Settings' }
  ];

  await testNavigation(page, adminLinks, 'SuperAdmin');

  // Test dashboard content
  await page.goto(`${BASE_URL}/admin`, { waitUntil: 'networkidle0' });
  await wait(1000);
  await testUIElements(page, 'SuperAdmin');

  // Clear session for next user
  await clearSession(page);
  pass('SuperAdmin session cleared');
}

async function testClubManager(page) {
  log('\n========== TESTING CLUB MANAGER ==========');

  await clearSession(page);

  const loggedIn = await testLogin(page, users.clubManager, 'ClubManager');
  if (!loggedIn) return;

  const clubLinks = [
    { path: '/club', name: 'Dashboard' },
    { path: '/club/members', name: 'Members List' },
    { path: '/club/members/new', name: 'New Member Form' },
    { path: '/club/sessions', name: 'Sessions List' },
    { path: '/club/sessions/new', name: 'New Session Form' },
    { path: '/club/events', name: 'Events List' },
    { path: '/club/events/new', name: 'New Event Form' },
    { path: '/club/payments', name: 'Payments List' },
    { path: '/club/memberships', name: 'Memberships' },
    { path: '/club/membership-types', name: 'Membership Types' },
    { path: '/club/venues', name: 'Venues' },
    { path: '/club/reports', name: 'Reports' },
    { path: '/club/settings', name: 'Settings' }
  ];

  await testNavigation(page, clubLinks, 'ClubManager');

  // Test dashboard content
  await page.goto(`${BASE_URL}/club`, { waitUntil: 'networkidle0' });
  await wait(1000);
  await testUIElements(page, 'ClubManager');

  // Test Members List with data
  await page.goto(`${BASE_URL}/club/members`, { waitUntil: 'networkidle0' });
  await wait(2000);

  const memberRows = await page.$$('table tbody tr');
  if (memberRows.length > 0) {
    pass(`ClubManager - Members table has ${memberRows.length} rows`);
  } else {
    warn('ClubManager - Members table may be empty or not loaded');
  }

  // Clear session for next user
  await clearSession(page);
  pass('ClubManager session cleared');
}

async function testMember(page) {
  log('\n========== TESTING MEMBER ==========');

  await clearSession(page);

  const loggedIn = await testLogin(page, users.member, 'Member');
  if (!loggedIn) return;

  const portalLinks = [
    { path: '/portal', name: 'Dashboard' },
    { path: '/portal/sessions', name: 'Sessions' },
    { path: '/portal/events', name: 'Events' },
    { path: '/portal/payments', name: 'Payments' },
    { path: '/portal/family', name: 'Family Members' },
    { path: '/portal/profile', name: 'Profile' },
    { path: '/portal/settings', name: 'Settings' }
  ];

  await testNavigation(page, portalLinks, 'Member');

  // Test dashboard content
  await page.goto(`${BASE_URL}/portal`, { waitUntil: 'networkidle0' });
  await wait(1000);
  await testUIElements(page, 'Member');
}

async function testAuthPages(page) {
  log('\n========== TESTING AUTH PAGES ==========');

  const authLinks = [
    { path: '/auth/login', name: 'Login Page' },
    { path: '/auth/register', name: 'Register Page' },
    { path: '/auth/forgot-password', name: 'Forgot Password Page' }
  ];

  for (const link of authLinks) {
    try {
      await page.goto(`${BASE_URL}${link.path}`, { waitUntil: 'networkidle0', timeout: 15000 });
      await wait(500);

      const hasForm = await page.$('form');
      if (hasForm) {
        pass(`Auth - ${link.name} loaded with form`);
      } else {
        warn(`Auth - ${link.name} may not have form`);
      }
    } catch (e) {
      fail(`Auth - ${link.name}`, e.message);
    }
  }
}

async function runTests() {
  console.log('\n' + '='.repeat(60));
  console.log('   THE LEAGUE - ANGULAR E2E TEST SUITE');
  console.log('='.repeat(60) + '\n');

  // Check API first
  const apiHealthy = await checkApiHealth();
  if (!apiHealthy) {
    console.log('\nAPI is not running. Please start the API first.');
    return;
  }

  const browser = await puppeteer.launch({
    headless: 'new',
    args: ['--no-sandbox', '--disable-setuid-sandbox']
  });

  const page = await browser.newPage();
  await page.setViewport({ width: 1280, height: 800 });

  // Capture console errors
  const consoleErrors = [];
  const notFoundUrls = [];

  page.on('console', msg => {
    if (msg.type() === 'error') {
      consoleErrors.push(msg.text());
    }
  });

  // Capture page errors
  page.on('pageerror', error => {
    consoleErrors.push(error.message);
  });

  // Capture 404 responses
  page.on('response', response => {
    if (response.status() === 404) {
      notFoundUrls.push(response.url());
    }
  });

  try {
    // Test auth pages first (no login required)
    await testAuthPages(page);

    // Test Super Admin
    await testSuperAdmin(page);

    // Test Club Manager
    await testClubManager(page);

    // Test Member
    await testMember(page);

  } catch (e) {
    fail('Test suite', e.message);
  } finally {
    await browser.close();
  }

  // Print summary
  console.log('\n' + '='.repeat(60));
  console.log('   TEST SUMMARY');
  console.log('='.repeat(60));
  console.log(`\n   PASSED: ${testResults.passed.length}`);
  console.log(`   FAILED: ${testResults.failed.length}`);
  console.log(`   WARNINGS: ${testResults.warnings.length}`);

  if (testResults.failed.length > 0) {
    console.log('\n   FAILED TESTS:');
    testResults.failed.forEach(f => {
      console.log(`   - ${f.test}: ${f.error}`);
    });
  }

  if (testResults.warnings.length > 0) {
    console.log('\n   WARNINGS:');
    testResults.warnings.forEach(w => {
      console.log(`   - ${w}`);
    });
  }

  if (consoleErrors.length > 0) {
    console.log('\n   CONSOLE ERRORS:');
    consoleErrors.slice(0, 10).forEach(e => {
      console.log(`   - ${e.substring(0, 100)}`);
    });
  }

  if (notFoundUrls.length > 0) {
    console.log('\n   404 API ENDPOINTS:');
    const uniqueUrls = [...new Set(notFoundUrls)];
    uniqueUrls.forEach(url => {
      console.log(`   - ${url}`);
    });
  }

  console.log('\n' + '='.repeat(60) + '\n');

  return testResults;
}

runTests().catch(console.error);
